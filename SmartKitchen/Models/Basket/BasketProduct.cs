using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
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