using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductCreation
	{
		public string Name { get; set; }
		public int Storage { get; set; }
	}
}