using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IProductRepository
    {
        Product GetProductByName(string name);
        IQueryable<Product> GetAllProducts();
    }
}
