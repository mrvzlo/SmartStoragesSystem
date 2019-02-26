using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketProductService
    {
        ItemCreationResponse AddBasketProduct(BasketProductCreationModel model, string email);
        BasketProductDisplayModel GetBasketProductDisplayModelById(int id, string email);
    }
}
