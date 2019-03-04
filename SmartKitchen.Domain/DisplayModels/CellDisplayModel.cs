using System;
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
        public Safety SafetyStatus { get {
                if (BestBefore == null) return Safety.Unknown;
                var days = (int)Math.Floor((BestBefore.Value.Date - DateTime.UtcNow.Date).TotalDays);
                return days > 1 ? Safety.IsSafe
                    : days > 0 ? Safety.ExpiresTomorrow
                    : days == 0 ? Safety.ExpiresToday
                    : Safety.Expired;
            }}


    }
}
