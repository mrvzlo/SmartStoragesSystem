using System.Data.Entity;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }
        public virtual DbSet<StorageType> StorageTypes { get; set; }
        public virtual DbSet<Cell> Cells { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<BasketProduct> BasketProducts { get; set; }
    }
}
