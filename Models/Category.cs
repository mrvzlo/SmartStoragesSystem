using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public sealed class Category
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public Category()
		{
			Id = 0;
			Name = "None";
		}
	}
}