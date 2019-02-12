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

        #region Create and Read  
        [HttpPost]
		public ActionResult Create(ProductCreation product)
		{
			product.Name = product.Name[0].ToString().ToUpper() + product.Name.Substring(1).ToLower();
			var storage = product.Storage;
			using (var db = new Context())
			{
				var productId = GetOrCreateAndGet(product.Name, db).Id;
                if (!db.Cells.Any(x => x.Product == productId))
                {
                    db.Cells.Add(new Cell {Product = productId, Amount = 0, BestBefore = null, Storage = storage});
                    db.SaveChanges();
                }
            }
			return Redirect(Url.Action("View", "Storage", new { id = storage }));
		}

		public Product GetOrCreateAndGet(string name, Context db)
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
        #endregion

        #region Amount
        public ActionResult AmountUpdatePre()
        {
            return PartialView("_AmountPicker");
        }

        public ActionResult SetAmount(int cell, int amount)
		{
			ProductDescription description = new ProductDescription();
			using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                if (cellold.Amount == Amount.None || amount == (int)Amount.None) cellold.BestBefore = null;
                if (amount > (int)Amount.Plenty) cellold.Amount = Amount.Plenty;
				else if (amount < (int)Amount.None) cellold.Amount = Amount.None;
				else cellold.Amount = (Amount)amount;
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
        #endregion

        #region Date

        public ActionResult DateUpdatePre()
        {
            return PartialView("_DatePicker");
        }
        
        public ActionResult DateUpdate(int cell, string dateStr)
        {
            DateTime? newDate;
            try
            {
                newDate = DateTime.ParseExact(dateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                newDate = null;
            }
            ProductDescription description = new ProductDescription();
            using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                if (cellold != null)
                {
                    if (newDate != null) cellold.BestBefore = newDate;
                    db.SaveChanges();
                    description = new ProductDescription(db, cell);
                }
            }

            return PartialView("_Description", description);
        }
        #endregion
    }
}