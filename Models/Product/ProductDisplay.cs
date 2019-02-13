using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDisplay
	{
		[Key]
		public int Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
    }
}