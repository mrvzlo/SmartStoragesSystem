using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.Enitities
{
	public class Cell
	{
		[Key]
		public virtual int Id { get; set; }
		public virtual int ProductId { get; set; }
		public virtual int Storage { get; set; }
		public virtual Amount Amount { get; set; }
		public virtual DateTime? BestBefore { get; set; }

        public virtual Product Product { get; set; }
	}
}