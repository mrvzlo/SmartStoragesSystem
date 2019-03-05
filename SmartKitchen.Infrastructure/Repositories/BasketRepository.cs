using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _dbContext;

        public BasketRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public Basket GetBasketById(int id) =>
            _dbContext.Baskets.Find(id);

        public IQueryable<Basket> GetBaskets() =>
            _dbContext.Baskets;

        public void AddOrUpdateBasket(Basket basket) =>
            _dbContext.InsertOrUpdate(basket);

        public void DeleteBasket(Basket basket) =>
            _dbContext.Delete(basket);

    }
}
