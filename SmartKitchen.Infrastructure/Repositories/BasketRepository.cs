using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _dbContext;
        
        public Basket GetBasketById(int id) =>
            _dbContext.Baskets.Find(id);

        public Basket GetBasketByNameAndOwner(string name, int owner) =>
            _dbContext.Baskets.FirstOrDefault(x => x.Owner == owner && x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public void AddBasket(Basket basket) => 
            _dbContext.InsertOrUpdate(basket);

        public void LockBasketById(int id) =>
            _dbContext.Baskets.Where(x => x.Id == id).Update(x => new Basket {Closed = true});

        public void DeleteBasket(Basket basket) =>
            _dbContext.Delete(basket);
        
    }
}
