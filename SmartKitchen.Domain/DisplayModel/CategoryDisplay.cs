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
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
	}
}