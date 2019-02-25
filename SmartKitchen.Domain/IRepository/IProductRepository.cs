using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface IProductRepository
    {
        Product GetProductByName(string name);
        IQueryable<Product> GetAllProducts();
        void ReplaceCategory(int fromId, int toId);
    }
}
