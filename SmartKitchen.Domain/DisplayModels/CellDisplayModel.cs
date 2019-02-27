﻿using System;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.DisplayModels
{
    public class CellDisplayModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public DateTime? BestBefore { get; set; }
        public Amount Amount { get; set; }
        public Notification SafetyStatus { get; set; }
        public Notification AmountStatus { get; set; }
    }
}
