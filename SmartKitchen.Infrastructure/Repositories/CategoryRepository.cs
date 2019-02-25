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
    }
}
