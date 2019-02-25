using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
    }
}
