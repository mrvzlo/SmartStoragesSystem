using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        void AddCategory(Category category);
        void RemoveCategoryById(int id);
        IQueryable<Category> GetAllCategories();
    }
}
