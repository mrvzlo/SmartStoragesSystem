using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IProductService
    {
        Product GetOrAddAndGet(string name);
        Product GetProductByName(string name);
        IQueryable<ProductDisplayModel> GetAllProductDisplays();
        ItemCreationResponse AddProduct(NameCreationModel model);
        void UpdateProductList(List<ProductDisplayModel> list);
    }
}
