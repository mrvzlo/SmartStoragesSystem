using System.Collections.Generic;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Linq;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketService
    {
        IQueryable<BasketDisplayModel> GetBasketsByOwnerEmail(string email);
        BasketDisplayModel GetBasketById(int id, string email);
        ItemCreationResponse AddBasket(NameCreationModel name, string email);
        BasketDisplayModel LockBasket(int id, string email);
        bool DeleteBasket(int id, string email);
        int FinishAndCloseBasket(int id, string email);
    }
}
