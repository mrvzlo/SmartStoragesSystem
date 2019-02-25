using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category GetCategoryById(int id) =>
            _dbContext.Categories.Find(id);

        public Category GetCategoryByName(string name) =>
            _dbContext.Categories.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public IQueryable<Category> GetAllCategories() =>
            _dbContext.Categories;

        public void AddCategory(Category category)
        {
            _dbContext.InsertOrUpdate(category);
        }

        public void RemoveCategoryById(int id)
        {
            _dbContext.Delete(_dbContext.Categories.Find(id));
        }
    }
}
