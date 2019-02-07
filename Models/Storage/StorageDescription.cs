using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartKitchen.Models
{
	public class StorageDescription
	{
		[Key]
		public int Id { get; }
		public string Name { get; set; }
		public StorageType Type { get; set; }
		public List<ProductDescription> Products { get; set; }

		public StorageDescription()
		{
			Type = new StorageType();
			Products = new List<ProductDescription>();
		}

		public StorageDescription(Storage storage, Context db)
		{
			Id = storage.Id;
			Name = storage.Name;
			Type = db.StorageTypes.Find(storage.Type);
			Products = new List<ProductDescription>();
			var list = db.ProductStatuses.Where(x => x.Storage == storage.Id).ToList();
			foreach (var status in list)
			{
				var product = db.Products.Find(status.Product);
				var category = db.Categories.Find(product.Category);
				Products.Add(new ProductDescription(status, product, category));
			}
		}
	}
}