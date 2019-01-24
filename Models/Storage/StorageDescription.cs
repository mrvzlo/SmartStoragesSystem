using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class StorageDescription
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Background { get; set; }
		public virtual string Icon { get; set; }

		public static string GetBackgroundName(StorageBackground id)
		{
			switch (id)
			{
				case StorageBackground.New: return "";
				default: return null;
			}
		}
		public static string GetIconName(StorageIcon id)
		{
			switch (id)
			{
				case StorageIcon.New: return "";
				default: return null;
			}
		}
	}
}