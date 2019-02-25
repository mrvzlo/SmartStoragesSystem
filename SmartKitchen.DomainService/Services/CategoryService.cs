using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Models;

namespace SmartKitchen.DomainService.Services
{
    class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryDisplay GetCategoryDisplayById(int id) => 
            Mapper.Map<CategoryDisplay>(_categoryRepository.GetCategoryById(id));
    }
}
