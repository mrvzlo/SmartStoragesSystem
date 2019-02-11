using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SmartKitchen.Enums;
using SmartKitchen.Models;

namespace SmartKitchen.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		public PartialViewResult _AddProduct(int storage)
		{
			ViewBag.storage = storage;
			return PartialView(new ProductCreation());
		}

		[HttpPost]
		public ActionResult Create(ProductCreation product)
		{
			product.Name = product.Name[0].ToString().ToUpper() + product.Name.Substring(1).ToLower();
			var storage = product.Storage;
			using (var db = new Context())
			{
				var productId = GetOrCreate(product.Name, db).Id;
				db.Cells.Add(new Cell { Product = productId, Amount = 0, BestBefore = null, Storage = storage});
				db.SaveChanges();
			}
			return Redirect(Url.Action("View", "Storage", new { id = storage }));
		}

		public Product GetOrCreate(string name, Context db)
		{
			var result = db.Products.FirstOrDefault(x => x.Name == name);
			if (result != null) return result;
			db.Products.Add(new Product {Category = 1, Name = name});
			db.SaveChanges();
			return db.Products.FirstOrDefault(x => x.Name == name);
		}

        public ActionResult Description(int cell)
        {
            ProductDescription description = new ProductDescription();
            using (var db = new Context())
            {
                description = new ProductDescription(db, cell);
            }

            return PartialView("_Description", description);
        }

        public ActionResult Change(int cell, int amount)
		{
			ProductDescription description = new ProductDescription();
			using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                if (cellold.Amount == Amount.None) cellold.BestBefore = null;
                if (cellold.Amount + amount > Amount.Plenty) cellold.Amount = Amount.Plenty;
				else if (cellold.Amount + amount < Amount.None) cellold.Amount = Amount.None;
				else cellold.Amount += amount;
                if (cellold.Amount == Amount.None) cellold.BestBefore = null;
				db.SaveChanges();
                description = new ProductDescription(db, cell);
            }

			return PartialView("_Description", description);
		}

        public ActionResult DateUpdate(int cell, string dateStr)
        {
            DateTime newDate = DateTime.ParseExact(dateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            ProductDescription description = new ProductDescription();
            using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                cellold.BestBefore = newDate;
                db.SaveChanges();
                description = new ProductDescription(db, cell);
            }

            return PartialView("_Description", description);
        }

        public void Remove(int cellId)
		{
			using (var db = new Context())
			{
				var cell = db.Cells.Find(cellId);
                if (cell == null) return;
				db.Cells.Remove(cell);
				db.SaveChanges();
			}
		}
	}
}