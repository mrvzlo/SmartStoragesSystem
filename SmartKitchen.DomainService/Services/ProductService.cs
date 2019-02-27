using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public Product GetOrAddAndGet(string name)
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
            if (GetProductByName(model.Name) != null) response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
            else
            {
                var product = new Product
                {
                    CategoryId = 1,
                    Name = TitledString(model.Name)
                };
                _productRepository.AddProduct(product);
                if (product.Id > 0) response.AddedId = product.Id;
                else response.AddError(GeneralError.AnErrorOccured);
            }
            return response;
        }

        public Product GetProductByName(string name) => 
            _productRepository.GetProductByName(name);

        public IQueryable<ProductDisplayModel> GetAllProductDisplays() => 
            _productRepository.GetAllProducts().ProjectTo<ProductDisplayModel>(MapperConfig);

        public void UpdateProductList(List<ProductDisplayModel> list)
        {
            foreach (var item in list)
            {
                var product = new Product
                {
                    Name = item.Name
                };
                if (_categoryService.ExistsWithId(item.CategoryId))
                    product.CategoryId = item.CategoryId;
                _productRepository.UpdateProduct(product);
            }
        }
    }
}
