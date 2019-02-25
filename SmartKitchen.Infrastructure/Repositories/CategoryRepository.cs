using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

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
    }
}
