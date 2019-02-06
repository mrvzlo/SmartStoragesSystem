using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartKitchen.Models
{
	public class StorageDescription
	{
		[Key]
		public int Id { get; }
		public string Name { get; set; }
		public StorageType Type { get; set; }
		public List<ProductDesctiption> Products { get; set; }

		public StorageDescription() { }

		public StorageDescription(Storage storage, Context db)
		{
			Id = storage.Id;
			Name = storage.Name;
			Type = db.StorageTypes.Find(storage.Type);
			Products = new List<ProductDesctiption>();
			foreach (var p in db.ProductStatuses.Where(x => x.Storage == storage.Id))
				Products.Add(new ProductDesctiption(p,db));
		}
	}
}