using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.Mvc;
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
			product.Name = product.Name[0].ToString().ToUpper() + product.Name.Substring(1).ToUpper();
			var storage = product.Storage;
			using (var db = new Context())
			{
				var productId = GetOrCreate(product.Name, db).Id;
				db.ProductStatuses.Add(new ProductStatus { Product = productId, Amount = 0, BestBefore = SqlDateTime.MinValue.Value, Storage = storage});
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
	}
}