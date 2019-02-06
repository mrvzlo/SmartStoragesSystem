using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDesctiption
	{
		public Product Product { get; set; }
		public ProductStatus Status { get; set; }
		public Safety Safety { get; set; }

		public ProductDesctiption(ProductStatus productStatus, Context db)
		{
			Status = productStatus;
			Product = db.Products.Find(productStatus.Product);
			GetSafety();
		}

		private void GetSafety()
		{
			double days = (DateTime.UtcNow.Date - Status.BestBefore.Date).TotalDays;
			if (days > 1) Safety = Safety.IsSafe;
			else if (days > 0) Safety = Safety.ExpiresTomorrow;
			else if (days == 0) Safety = Safety.ExpiresToday;
			else Safety = Safety.Expired;
		}
	}
}