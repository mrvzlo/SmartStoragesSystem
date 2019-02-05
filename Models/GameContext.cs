using System.Data.Entity;

namespace SmartKitchen.Models
{
	public class Context : DbContext
	{
		public Context() : base("DefaultConnection") { }
		public DbSet<Person> People { get; set; }
		public DbSet<Storage> Storages { get; set; }
		public DbSet<StorageType> StorageTypes { get; set; }
	}
}