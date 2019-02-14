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
		public List<int> Products{ get; set; }

		public StorageDescription()
		{
			Type = new StorageType();
            Products = new List<int>();
		}

		public StorageDescription(Storage storage, Context db)
		{
			Id = storage.Id;
			Name = storage.Name;
			Type = db.StorageTypes.Find(storage.Type);
            Products = db.Cells.Where(x => x.Storage == storage.Id).Select(x => x.Id).ToList();
		}
	}
}