﻿using System.Data.Entity;

namespace SmartKitchen.Models
{
	public class Context : DbContext
	{
		public Context() : base("DefaultConnection") { }
		public DbSet<User> Users { get; set; }
		public DbSet<Storage> Storages { get; set; }
		public DbSet<StorageType> StorageTypes { get; set; }
	}
}