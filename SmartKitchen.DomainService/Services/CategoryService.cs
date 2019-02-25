using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;
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
