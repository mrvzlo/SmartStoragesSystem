using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        void AddCategory(Category category);
        void DeleteCategoryById(int id);
        IQueryable<Category> GetAllCategories();
        bool ExistsWithId(int id);
    }
}
