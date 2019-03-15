using System;

namespace SmartKitchen.Domain.DisplayModels
{
    public class BasketDisplayModel
    {
        public int Id { get; set; }
        public bool Closed { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int Products { get; set; }
        public int BoughtProducts { get; set; }
        public decimal FullPrice { get; set; }
    }
}