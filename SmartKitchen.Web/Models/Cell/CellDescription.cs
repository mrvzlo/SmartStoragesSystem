using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class CellDescription
	{
		public Product Product { get; set; }
		public Cell Cell { get; set; }
        public Notification Safety { get; set; }
        public Notification Amount { get; set; }
        public Category Category{ get; set; }

        public CellDescription()
        {
            Product = new Product();
            Cell = new Cell();
            Category = new Category();
            Safety = new Notification();
            Amount = new Notification();
        }
        public CellDescription(Context db, int cell)
        {
            Cell = db.Cells.Find(cell);
            Product = db.Products.Find(Cell.Product);
            Category = db.Categories.Find(Product.Category);
            Safety = new Notification(Cell.BestBefore);
            Amount = new Notification(Cell.Amount);
        }

        public CellDescription(Cell cell, Product product, Category category)
		{
            Cell = cell;
			Product = product;
			Category = category;
            Safety = new Notification(Cell.BestBefore);
            Amount = new Notification(Cell.Amount);
        }

	}
}