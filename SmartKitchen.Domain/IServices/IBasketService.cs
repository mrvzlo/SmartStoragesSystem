using SmartKitchen.Domain.DisplayModel;
using System.Collections.Generic;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketService
    {
        List<BasketDisplayModel> GetBasketsWithDescriptionByOwnerEmail(string email);
        ItemCreationResponse CreateBasket(NameCreationModel name, string email);
    }
}
