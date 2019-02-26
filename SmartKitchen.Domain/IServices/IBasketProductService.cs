using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IBasketProductService
    {
        ItemCreationResponse AddBasketProduct(BasketProductCreationModel model, string email);
    }
}
