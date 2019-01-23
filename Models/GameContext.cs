using System.Data.Entity;

namespace SmartKitchen.Models
{
	public class Context : DbContext
	{
		public Context() : base("DefaultConnection") { }
		public DbSet<User> Users { get; set; }
	}
}