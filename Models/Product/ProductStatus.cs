using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductStatus
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual int Product { get; set; }
		public virtual int Storage { get; set; }
		public virtual Amount Amount { get; set; }
		public virtual DateTime? BestBefore { get; set; }
	}
}