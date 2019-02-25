using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
    }
}
