using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public CategoryDisplay GetCategoryDisplayById(int id) =>
            Mapper.Map<CategoryDisplay>(_categoryRepository.GetCategoryById(id));

        public List<CategoryDisplay> GetAllCategoryDisplays() =>
            _categoryRepository.GetAllCategories().ProjectTo<CategoryDisplay>(MapperConfig).ToList();

        public ServiceResponse AddCategoryWithName(string name)
        {
            var response = new ServiceResponse();
            name = name.Trim();
            var category = _categoryRepository.GetCategoryByName(name);
            if (category != null)
            {
                response.Errors.Add(new ModelStateError("Name", FormError.NameIsAlreadyTaken));
                return response;
            }
            _categoryRepository.AddCategory(new Category
            {
                Name = name
            });
            return response;
        }

        public void ReplaceCategory(int fromId, int toId)
        {
            var fromCategory = _categoryRepository.GetCategoryById(fromId);
            var toCategory = _categoryRepository.GetCategoryById(toId);
            if (fromCategory == null || toCategory == null || fromId == 1) return;
            _productRepository.ReplaceCategory(fromId, toId);
            _categoryRepository.RemoveCategoryById(fromId);
        }
    }
}
