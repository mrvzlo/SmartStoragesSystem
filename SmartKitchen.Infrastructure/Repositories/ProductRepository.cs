using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
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

        public void AddProduct(Product product) =>
            _dbContext.InsertOrUpdate(product);

        public void ReplaceCategory(int fromId, int toId) => 
            _dbContext.Products.Where(x => x.Category == fromId).Update(x => new Product { Category = toId });

        public Product GetProductById(int id) =>
            _dbContext.Products.Find(id);
    }
}
