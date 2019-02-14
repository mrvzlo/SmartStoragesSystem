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
	public class CellController : Controller
	{
        #region Create and Read  
        public PartialViewResult _AddProduct(int storage)
        {
            ViewBag.storage = storage;
            return PartialView(new CellCreation());
        }

        [HttpPost]
		public ActionResult Create(CellCreation product)
        {
			var storage = product.Storage;
			using (var db = new Context())
			{
				var productId = GetOrCreateAndGet(product.Name, db).Id;
                if (!db.Cells.Any(x => x.Product == productId && x.Storage == storage))
                {
                    db.Cells.Add(new Cell {Product = productId, Amount = 0, BestBefore = null, Storage = storage});
                    db.SaveChanges();
                }
            }
			return Redirect(Url.Action("View", "Storage", new { id = storage }));
		}

		public Product GetOrCreateAndGet(string name, Context db)
        {
            name = name.Trim();
			var result = db.Products.FirstOrDefault(x => x.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
			if (result != null) return result;
			db.Products.Add(new Product {Category = 1, Name = name});
			db.SaveChanges();
			return db.Products.FirstOrDefault(x => x.Name == name);
		}

        public PartialViewResult Description(int cell)
        {
            CellDescription description = new CellDescription();
            using (var db = new Context())
            {
                description = new CellDescription(db, cell);
            }
            return PartialView("_Description", description);
        }
        #endregion

        #region Amount
        public PartialViewResult AmountUpdatePre()
        {
            return PartialView("_AmountPicker");
        }

        public PartialViewResult SetAmount(int cell, int amount)
		{
            CellDescription description = new CellDescription();
			using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                if (cellold.Amount == Amount.None || amount == (int)Amount.None) cellold.BestBefore = null;
                if (amount > (int)Amount.Plenty) cellold.Amount = Amount.Plenty;
				else if (amount < (int)Amount.None) cellold.Amount = Amount.None;
				else cellold.Amount = (Amount)amount;
				db.SaveChanges();
                description = new CellDescription(db, cell);
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

        public PartialViewResult DateUpdatePre()
        {
            return PartialView("_DatePicker");
        }
        
        public PartialViewResult DateUpdate(int cell, string dateStr)
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
            CellDescription description = new CellDescription();
            using (var db = new Context())
            {
                Cell cellold = db.Cells.Find(cell);
                if (cellold != null)
                {
                    if (newDate != null) cellold.BestBefore = newDate;
                    db.SaveChanges();
                    description = new CellDescription(db, cell);
                }
            }

            return PartialView("_Description", description);
        }
        #endregion
    }
}