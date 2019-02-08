using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDescription
	{
		public Product Product { get; set; }
		public ProductStatus Status { get; set; }
		public Notification SafetyNotify { get; set; }
		public Notification AmountNotify { get; set; }
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
			SafetyNotify = new Notification();
			if (productStatus.Amount != Amount.None)
			SafetyNotify = new Notification(productStatus.BestBefore);
			AmountNotify = new Notification(productStatus.Amount);
			if (Product.Category == 0) Category = new Category();
		}

	}
}