using System;
using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product GetProductByName(string name) =>
            _dbContext.Products.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public IQueryable<Product> GetAllProducts() =>
            _dbContext.Products;
    }
}
