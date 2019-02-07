using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDescription
	{
		public Product Product { get; set; }
		public ProductStatus Status { get; set; }
		public Safety Safety { get; set; }
		public Category Category{ get; set; }

		public ProductDescription()
		{
			Product = new Product();
			Status = new ProductStatus();
			Category = new Category();
		}

		public ProductDescription(ProductStatus productStatus, Product product, Category category)
		{
			Status = productStatus;
			Product = product;
			Category = category;
			Safety = GetSafety();
			if (Product.Category == 0) Category = new Category();
		}

		private Safety GetSafety()
		{
			var days = (int)Math.Floor((Status.BestBefore.Date - DateTime.UtcNow.Date).TotalDays);
			return days > 1 ? Safety.IsSafe 
				: days > 0 ? Safety.ExpiresTomorrow
				: days == 0 ? Safety.ExpiresToday 
				: Safety.Expired;
		}
	}
}