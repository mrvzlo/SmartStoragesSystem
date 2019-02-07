using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class Product
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Category { get; set; }
	}
}