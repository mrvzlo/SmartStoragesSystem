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
        [HttpPost]
		public ActionResult Create(CellCreation product)
        {
            Storage storage;
			using (var db = new Context())
            {
                storage = db.Storages.Find(product.Storage);
                var response = Storage.IsOwner(storage, Person.Current(db));
                if (!response.Successfull) return Redirect(Url.Action("Index", "Error", new {id = response.Error}));
				var productId = GetOrCreateAndGet(product.Name, db).Id;
                if (db.Cells.Any(x => x.Product == productId && x.Storage == storage.Id))
                    TempData["error"] = "This name is already taken";
                else
                {
                    db.Cells.Add(new Cell {Product = productId, Amount = 0, BestBefore = null, Storage = storage.Id});
                    db.SaveChanges();
                }
            }
			return Redirect(Url.Action("View", "Storage", new { id = storage.Id }));
		}

		private Product GetOrCreateAndGet(string name, Context db)
        {
            name = name.Trim();
			var result = db.Products.FirstOrDefault(x => x.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
			if (result != null) return result;
			db.Products.Add(new Product {Category = 1, Name = name});
			db.SaveChanges();
			return db.Products.FirstOrDefault(x => x.Name == name);
		}
    }
}