using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IProductRepository
    {
        Product GetProductByName(string name);
        IQueryable<Product> GetAllProducts();
        void ReplaceCategory(int fromId, int toId);
        void AddProduct(Product product);
        Product GetProductById(int id);
        void UpdateProduct(Product product);
        bool ExistsAnotherWithEqualName(string name, int id);
    }
}
