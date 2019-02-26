using System;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _dbContext;

        public IQueryable<Basket> GetAllUserBaskets(int personId) =>
            _dbContext.Baskets.Where(x => x.Owner == personId).OrderBy(x => x.Closed).ThenByDescending(x => x.CreationDate);

        public Basket GetBasketByNameAndOwner(string name, int owner) =>
            _dbContext.Baskets.FirstOrDefault(x => x.Owner == owner && x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public void AddBasket(Basket basket)
        {
            _dbContext.Baskets.Add(basket); //todo
            _dbContext.SaveChanges();
        }
    }
}
