using System;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.DisplayModels
{
    public class CellDisplayModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public DateTime? BestBefore { get; set; }

        public int Amount { get; set; }

        public ICollection<CellChange> CellChanges { get; set; }

        public Amount AmountStatus { get {
            if (Amount == 0) return Enums.Amount.None;
            if (AmountDecreasePerHour == 0) return Enums.Amount.Plenty;
            var hoursRemain = Amount / AmountDecreasePerHour;
            if (hoursRemain == 0) return Enums.Amount.None;
            if (hoursRemain < 48) return Enums.Amount.Lack;
            return Enums.Amount.Plenty;
        }}

        public Safety SafetyStatus { get {
                if (BestBefore == null) return Safety.Unknown;
                var days = (int)Math.Floor((BestBefore.Value.Date - DateTime.Now.Date).TotalDays);
                return days > 1 ? Safety.IsSafe
                    : days > 0 ? Safety.ExpiresTomorrow
                    : days == 0 ? Safety.ExpiresToday
                    : Safety.Expired;
            }}


        public decimal AmountDecreasePerHour{ get {
                decimal decrease = 0, hours = 0;
                int count = 0;
                var changeslist = CellChanges.OrderByDescending(x => x.UpdateDate);
                foreach (var newest in changeslist)
                {
                    var oldest = changeslist.FirstOrDefault(x => x.UpdateDate < newest.UpdateDate);
                    if (oldest == null || oldest.Amount <= newest.Amount) continue;
                    count++;
                    decrease += newest.Amount - oldest.Amount;
                    hours += (decimal)(newest.UpdateDate - oldest.UpdateDate).TotalHours;
                    if (count == 3) break;
                }
                return hours == 0 ? 0 : Math.Round(Math.Abs(decrease/hours),2);
            }
        }}
}
