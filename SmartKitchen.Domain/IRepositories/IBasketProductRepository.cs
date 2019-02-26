using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IBasketProductRepository
    {
        void AddBasketProduct(BasketProduct basketProduct);
    }
}
