using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketProductRepository : IBasketProductRepository
    {
        private readonly AppDbContext _dbContext;

        public void AddBasketProduct(BasketProduct basketProduct) =>
            _dbContext.InsertOrUpdate(basketProduct);
    }
}
