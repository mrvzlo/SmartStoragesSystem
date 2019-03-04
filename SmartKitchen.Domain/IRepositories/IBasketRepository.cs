using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IBasketRepository
    {
        Basket GetBasketById(int id);
        void AddOrUpdateBasket(Basket basket);
        void DeleteBasket(Basket basket);
    }
}
