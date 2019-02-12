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
            var safety = GetSafety(bestBefore);
            AmountInfo = GetAmountInfo(amount);
            SafetyInfo = GetSafetyInfo(safety);
            Type = GetType(safety);
        }

        private string GetAmountInfo(Amount amount)
		{
			switch (amount)
			{
				case Amount.None: return "None";
				case Amount.Lack: return "Lack";
				case Amount.Plenty: return "Plenty";
                default: return "Unknown";
			}
		}

		private Safety GetSafety(DateTime? bestBefore)
		{
            if (bestBefore == null) return Safety.Unknown;
			var days = (int)Math.Floor((bestBefore.Value.Date - DateTime.UtcNow.Date).TotalDays);
            return days > 1 ? Safety.IsSafe 
                : days > 0 ? Safety.ExpiresTomorrow 
                    : days == 0 ? Safety.ExpiresToday 
                        : Safety.Expired;
        }

        private string GetSafetyInfo(Safety safety)
        {
            switch (safety)
            {
                case Safety.Expired: return "Expired";
                case Safety.ExpiresTomorrow: return "Expires tomorrow";
                case Safety.ExpiresToday: return "Expires today";
                case Safety.IsSafe: return "Safe";
                default: return "Unknown";
            }
        }

        private string GetType(Safety safety)
        {
            string[] colorThemes = { "danger", "warning", "success", "secondary", "primary", "main"};
            switch (safety)
            {
                case Safety.Expired: return colorThemes[0];
                case Safety.ExpiresTomorrow: 
                case Safety.ExpiresToday: return colorThemes[1];
                case Safety.IsSafe: return colorThemes[2];
                default: return colorThemes[3];
            }
        }
    }
}