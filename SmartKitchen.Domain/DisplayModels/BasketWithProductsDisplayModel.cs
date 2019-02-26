using System;
using System.Collections.Generic;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.DisplayModels
{
    public class BasketWithProductsDisplayModel
    {
        public int Id { get; set; }
        public bool Closed { get; set; }
        public string Name { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual List<BasketProduct> BasketProducts { get; set; }
    }
}
