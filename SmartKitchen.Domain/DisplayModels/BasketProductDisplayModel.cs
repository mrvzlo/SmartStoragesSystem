﻿using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.DisplayModels
{
    public class BasketProductDisplayModel
    {
        public int Id { get; set; }
        public int Cell { get; set; }
        public bool Bought { get; set; }
        public decimal Price { get; set; }
        public DateTime? BestBefore { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
    }
}