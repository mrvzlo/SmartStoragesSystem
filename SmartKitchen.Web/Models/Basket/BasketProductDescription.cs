using SmartKitchen.Domain.Enitities;
using System.ComponentModel.DataAnnotations;

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