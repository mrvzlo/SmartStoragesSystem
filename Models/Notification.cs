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
		public string Content { get; set; }

		public Notification()
		{
			Type = "secondary";
			Content = "-";
		}

		public Notification(DateTime? bestBefore)
		{
			if (bestBefore == null)
			{
				Type = "secondary";
				Content = "-";
				return;
			}
			switch (GetSafety(bestBefore.Value))
			{
				case Safety.IsSafe:
					Type = "success";
					Content = "Is safe";
					return;
				case Safety.ExpiresTomorrow:
					Type = "warning";
					Content = "Expires tomorrow";
					return;
				case Safety.ExpiresToday:
					Type = "warning";
					Content = "Expires today";
					return;
				case Safety.Expired:
					Type = "danger";
					Content = "expired";
					return;
			}
		}
		public Notification(Amount amount)
		{
			switch (amount)
			{
				case Amount.None:
					Type = "danger";
					Content = "None";
					return;
				case Amount.Lack:
					Type = "warning";
					Content = "Lack";
					return;
				case Amount.Plenty:
					Type = "success";
					Content = "Plenty";
					return;
			}
		}

		private Safety GetSafety(DateTime bestBefore)
		{
			var days = (int)Math.Floor((bestBefore.Date - DateTime.UtcNow.Date).TotalDays);
			return days > 1 ? Safety.IsSafe
				: days > 0 ? Safety.ExpiresTomorrow
				: days == 0 ? Safety.ExpiresToday
				: Safety.Expired;
		}
	}
}