using SmartKitchen.Domain.Enitities;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IBasketProductRepository
    {
        void AddBasketProduct(BasketProduct basketProduct);
        BasketProduct GetBasketProductById(int id);
        void DeleteBasketProductRange(ICollection<BasketProduct> query);
        BasketProduct GetBasketProductByBasketAndCell(int basket, int cell);
    }
}
