using System.Collections.Generic;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketProductService
    {
        ItemCreationResponse AddBasketProductByModel(BasketProductCreationModel model, string email);
        ItemCreationResponse AddBasketProduct(Storage storage, Basket basket, Person person, int cellId);
        BasketProductDisplayModel GetBasketProductDisplayModelById(int id, string email);
        int AddBasketProductList(int basketId, int storageId, string email, List<int> cells);
    }
}
