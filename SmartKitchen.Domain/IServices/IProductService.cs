using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IProductService
    {
        Product GetOrAddAndGet(string name);
        Product GetProductByName(string name);
        ItemCreationResponse AddProduct(string name);
    }
}
