using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public sealed class Notification
	{
		public string Type { get; set; }
        public string AmountInfo { get; set; }
        public string SafetyInfo { get; set; }

        public Notification()
		{
			Type = "secondary";
            AmountInfo = "None";
            SafetyInfo = "Unknown";
        }

		public Notification(DateTime? bestBefore, Amount amount)
        {
            GetAmountInfo(amount);
            GetSafety(bestBefore);
        }

        private void GetAmountInfo(Amount amount)
		{
			switch (amount)
			{
				case Amount.None:
                    AmountInfo = "None";
					return;
				case Amount.Lack:
                    AmountInfo = "Lack";
					return;
				case Amount.Plenty:
                    AmountInfo = "Plenty";
					return;
			}
		}

		private Safety GetSafety(DateTime? bestBefore)
		{
            if (bestBefore == null)
            {
                SafetyInfo = "Unknown";
                Type = "secondary";
                return Safety.Unknown;
            }
			var days = (int)Math.Floor((bestBefore.Value.Date - DateTime.UtcNow.Date).TotalDays);
			if (days > 1)
            {
                SafetyInfo = "Safe";
                Type = "success";
                return Safety.IsSafe;
            }
            if (days > 0)
            {
                SafetyInfo = "Expires tomorrow";
                Type = "warning";
                return Safety.ExpiresTomorrow;
            }
            if (days == 0)
            {
                SafetyInfo = "Expires today";
                Type = "warning";
                return Safety.ExpiresToday;
            }
            SafetyInfo = "Expired";
            Type = "danger";
            return Safety.Expired;
		}
	}
}