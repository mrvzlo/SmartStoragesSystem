using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketProductRepository : IBasketProductRepository
    {
        private readonly AppDbContext _dbContext;

        public BasketProductRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public void AddBasketProduct(BasketProduct basketProduct) =>
            _dbContext.InsertOrUpdate(basketProduct);

        public BasketProduct GetBasketProductById(int id) =>
            _dbContext.BasketProducts.Find(id);
    }
}
