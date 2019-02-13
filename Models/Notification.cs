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
        public string Info { get; set; }

        public Notification()
		{
            Type = "secondary";
            Info = "Unknown";
        }

		public Notification(Amount amount)
        {
            Info = GetAmountInfo(amount);
            Type = GetAmountType(amount);
        }
        public Notification(DateTime? bestBefore)
        {
            var safety = GetSafety(bestBefore);
            Info = GetSafetyInfo(safety);
            Type = GetSafetyType(safety);
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

        private string GetSafetyType(Safety safety)
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
        private string GetAmountType(Amount amount)
        {
            string[] colorThemes = { "danger", "warning", "success", "secondary", "primary", "main" };
            switch (amount)
            {
                case Amount.None: return colorThemes[3];
                case Amount.Lack: return colorThemes[1];
                case Amount.Plenty: return colorThemes[2];
                default: return colorThemes[3];
            }
        }
    }
}