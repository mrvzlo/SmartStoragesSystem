using System;

namespace SmartKitchen.Domain.DisplayModels
{
    public class BasketProductDisplayModel
    {
        public int Id { get; set; }
        public int CellId { get; set; }
        public bool Bought { get; set; }
        public decimal Price { get; set; }
        public DateTime? BestBefore { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public string StorageName { get; set; }
    }
}