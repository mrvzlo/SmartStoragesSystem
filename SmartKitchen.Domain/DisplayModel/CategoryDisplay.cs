using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class CategoryDisplay 
    {
        public Category Category { get; set; }
        public int ProductsCount { get; set; }
	}
}