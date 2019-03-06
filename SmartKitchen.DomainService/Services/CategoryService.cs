using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        
        public IQueryable<CategoryDisplay> GetAllCategoryDisplays() =>
            _categoryRepository.GetAllCategories().ProjectTo<CategoryDisplay>(MapperConfig);

        public ServiceResponse AddCategoryWithName(NameCreationModel model)
        {
            var response = new ServiceResponse();
            model.Name = model.Name.Trim();
            var exists = _categoryRepository.GetCategoryByName(model.Name) != null;
            if (exists)
            {
                response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
                return response;
            }
            _categoryRepository.AddCategory(new Category
            {
                Name = TitledString(model.Name)
            });
            return response;
        }

        public bool ReplaceCategory(int fromId, int toId)
        {
            var fromCategory = _categoryRepository.GetCategoryById(fromId);
            var toCategory = _categoryRepository.GetCategoryById(toId);
            if (fromCategory == null || toCategory == null || fromId == 1 || fromId == toId) return false;
            _productRepository.ReplaceCategory(fromId, toId);
            _categoryRepository.DeleteCategoryById(fromId);
            return true;
        }
    }
}
