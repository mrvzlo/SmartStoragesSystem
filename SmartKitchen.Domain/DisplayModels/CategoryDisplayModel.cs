﻿namespace SmartKitchen.Domain.DisplayModels
{
	public class CategoryDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
        public bool Primal { get; set; }
	}
}