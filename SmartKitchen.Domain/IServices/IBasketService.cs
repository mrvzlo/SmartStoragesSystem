using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketService
    {
        List<BasketDisplayModel> GetBasketsByOwnerEmail(string email);
        BasketDisplayModel GetBasketById(int id, string email);
        BasketWithProductsDisplayModel GetBasketWithProductsById(int id, string email);
        ItemCreationResponse CreateBasket(NameCreationModel name, string email);
        bool LockBasket(int id, string email);
        bool DeleteBasket(int id, string email);
    }
}
