using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketService
    {
        List<BasketDisplayModel> GetBasketsWithDescriptionByOwnerEmail(string email);
        ItemCreationResponse CreateBasket(NameCreationModel name, string email);
    }
}
