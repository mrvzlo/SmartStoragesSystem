using System;
using System.ComponentModel.DataAnnotations;
using SmartKitchen.Enums;

namespace SmartKitchen.Models
{
	public class ProductDescription
	{
		public Product Product { get; set; }
		public Cell Cell { get; set; }
		public Notification Notification { get; set; }
		public Category Category{ get; set; }

        public ProductDescription()
        {
            Product = new Product();
            Cell = new Cell();
            Category = new Category();
            Notification = new Notification();
        }
        public ProductDescription(Context db, int cell)
        {
            Cell = db.Cells.Find(cell);
            Product = db.Products.Find(Cell.Product);
            Category = db.Categories.Find(Product.Category);
            Notification = new Notification(Cell.BestBefore, Cell.Amount);
        }

        public ProductDescription(Cell cell, Product product, Category category)
		{
            Cell = cell;
			Product = product;
			Category = category;
            Notification = new Notification(cell.BestBefore, cell.Amount);
		}

	}
}