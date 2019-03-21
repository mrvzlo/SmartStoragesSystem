using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
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
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public Product GetOrAddAndGetProduct(string name)
        {
            var product = GetProductByName(name);
            if (product != null) return product;
            var creation = AddProduct(new NameCreationModel(name));
            return !creation.Successful() ? null : _productRepository.GetProductById(creation.AddedId);
        }

        public ItemCreationResponse AddProduct(NameCreationModel model)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            if (GetProductByName(model.Name) != null)
                return response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));

            var primalCategory = _categoryRepository.GetCategoryByName("").Id;
            var product = new Product
            {
                CategoryId = primalCategory,
                Name = TitledString(model.Name)
            };
            _productRepository.AddProduct(product);
            response.AddedId = product.Id;
            return response;
        }

        public Product GetProductByName(string name) =>
            _productRepository.GetProductByName(name);

        public IQueryable<ProductDisplayModel> GetAllProductDisplays() =>
            _productRepository.GetAllProducts().ProjectTo<ProductDisplayModel>(MapperConfig);

        public int UpdateProductList(List<ProductDisplayModel> list)
        {
            int count = 0;
            if (list == null) return count;
            foreach (var item in list)
            {
                var product = _productRepository.GetProductById(item.Id);
                if (product == null) continue;
                if (product.Name == item.Name && product.CategoryId == item.CategoryId) continue;
                count++;
                if (!_productRepository.ExistsAnotherWithEqualName(item.Name, item.Id))
                    product.Name = item.Name;
                if (_categoryRepository.ExistsWithId(item.CategoryId))
                    product.CategoryId = item.CategoryId;
                    _productRepository.UpdateProduct(product);
            }

            return count;
        }
    }
}
