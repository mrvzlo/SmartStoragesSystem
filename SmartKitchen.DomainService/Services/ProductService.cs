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

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetOrAddAndGet(string name)
        {
            var product = GetProductByName(name);
            if (product != null) return product;
            var creation = AddProduct(name);
            return !creation.Successful() ? null : _productRepository.GetProductById(creation.AddedId);
        }

        public ItemCreationResponse AddProduct(string name)
        {
            var response = new ItemCreationResponse();
            if (GetProductByName(name) != null) response.AddError(GeneralError.NameIsAlreadyTaken, "Name");
            else
            {
                var product = new Product
                {
                    CategoryId = 1,
                    Name = TitledString(name)
                };
                _productRepository.AddProduct(product);
                if (product.Id > 0) response.AddedId = product.Id;
                else response.AddError(GeneralError.AnErrorOccured);
            }
            return response;
        }

        public Product GetProductByName(string name) => 
            _productRepository.GetProductByName(name);

    }
}
