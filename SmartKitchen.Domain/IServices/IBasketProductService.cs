using System;
using System.Collections.Generic;
using System.Linq;
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
        IQueryable<BasketProductDisplayModel> GetBasketProductDisplayModelByBasket(int basketId, string email);
        int AddBasketProductList(int basketId, int storageId, string email, List<int> cells);
        ServiceResponse MarkProductBought(int id, string email);
        ServiceResponse UpdateProductPrice(int id, int value, string email);
        ServiceResponse UpdateProductAmount(int id, int value, string email);
        ServiceResponse UpdateProductBestBefore(int id, DateTime? value, string email);
        ServiceResponse DeleteBasketProductByIdAndEmail(int id, string email);
    }
}
