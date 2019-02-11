﻿using System.Data.Entity;

namespace SmartKitchen.Models
{
	public class Context : DbContext
	{
		public Context() : base("DefaultConnection") { }
		public DbSet<Person> People { get; set; }
		public DbSet<Storage> Storages { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Cell> Cells { get; set; }
		public DbSet<StorageType> StorageTypes { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}