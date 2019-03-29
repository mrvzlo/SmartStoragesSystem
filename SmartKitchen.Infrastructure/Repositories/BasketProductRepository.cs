using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class BasketProductRepository : IBasketProductRepository
    {
        private readonly AppDbContext _dbContext;

        public BasketProductRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public void AddOrUpdateBasketProduct(BasketProduct basketProduct) =>
            _dbContext.InsertOrUpdate(basketProduct);

        public BasketProduct GetBasketProductById(int id) =>
            _dbContext.BasketProducts.Find(id);

        public void DeleteBasketProductRange(ICollection<BasketProduct> query) =>
            _dbContext.BasketProducts.RemoveRange(query);

        public BasketProduct GetBasketProductByBasketAndCell(int basket, int cell) =>
            _dbContext.BasketProducts.SingleOrDefault(x => x.BasketId == basket && x.CellId == cell);

        public void DeleteBasketProduct(BasketProduct basketProduct) =>
            _dbContext.Delete(basketProduct);

        public void UnmarkBasketProducts(int basketId) =>
            _dbContext.BasketProducts.Where(x => x.BasketId == basketId)
                .Update(x => new BasketProduct {Bought = false});
    }
}
