using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface IProductRepository
    {
        Product GetProductByName(string name);
    }
}
