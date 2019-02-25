using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public Product GetProductByName(string name) =>
            _dbContext.Products.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public IQueryable<Product> GetAllProducts() =>
            _dbContext.Products;

        public void ReplaceCategory(int fromId, int toId)
        {
            _dbContext.Products.Where(x => x.Category == fromId).Update(x => new Product{ Category = toId});
            _dbContext.SaveChanges();
        }
    }
}
