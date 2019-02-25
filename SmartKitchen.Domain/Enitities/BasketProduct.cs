using System;
using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.Enitities
{
	public class BasketProduct
	{
		[Key]
        public virtual int Id { get; set; }
        public virtual int Cell { get; set; }
        public virtual bool Bought { get; set; }
        public virtual int Basket { get; set; }
        public virtual decimal Price{ get; set; }
        public virtual DateTime? BestBefore { get; set; }
    }
}