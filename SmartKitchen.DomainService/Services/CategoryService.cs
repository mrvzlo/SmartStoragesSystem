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

        public IQueryable<CategoryDisplayModel> GetAllCategoryDisplays() =>
            _categoryRepository.GetAllCategories().ProjectTo<CategoryDisplayModel>(MapperConfig);

        public ServiceResponse AddCategory(NameCreationModel model)
        {
            var response = new ServiceResponse();
            model.Name = model.Name.Trim();
            var exists = _categoryRepository.GetCategoryByName(model.Name) != null;
            if (exists) return response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
            _categoryRepository.AddCategory(new Category
            {
                Name = TitledString(model.Name)
            });
            return response;
        }

        public ServiceResponse ReplaceCategory(int fromId, int toId)
        {
            var response = new ServiceResponse();
            var initialCategory = _categoryRepository.GetAllCategories().Single(x => x.Name == "");
            var fromCategory = _categoryRepository.GetCategoryById(fromId);
            var toCategory = _categoryRepository.GetCategoryById(toId);
            if (fromCategory == null || toCategory == null) return response.AddError(GeneralError.CategoryWasNotFound);
            if (fromId == initialCategory.Id) return response.AddError(GeneralError.CantRemovePrimalCategory);
            if ( fromId == toId) return response.AddError(GeneralError.CantReplaceToItself);
            _productRepository.ReplaceCategory(fromId, toId);
            _categoryRepository.DeleteCategoryById(fromId);
            return response;
        }
    }
}
