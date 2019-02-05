using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class StorageDescription
	{
		[Key]
		public int Id { get; }
		public string Name { get; set; }
		public string TypeName { get; set; }
		public string Background { get; set; }
		public string Icon { get; set; }

		public StorageDescription() { }

		public StorageDescription(Storage storage, StorageType storageType)
		{
			Id = storage.Id;
			Name = storage.Name;
			TypeName = storageType.Name;
			Icon = storageType.Icon;
			Background = storageType.Background;
		}
	}
}