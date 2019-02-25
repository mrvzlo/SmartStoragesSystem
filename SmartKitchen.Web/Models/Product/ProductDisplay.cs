using System.ComponentModel.DataAnnotations;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDisplay
    {
        public Product Product { get; set; }
        public string CategoryName { get; set; }
        public int Usages { get; set; }
    }
}