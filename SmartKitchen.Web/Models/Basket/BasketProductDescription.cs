using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class BasketProductDescription
	{
		[Key]
        public BasketProduct Product { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Amount { get; set; }
    }
}