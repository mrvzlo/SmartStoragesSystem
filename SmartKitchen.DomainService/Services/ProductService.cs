using SmartKitchen.Domain.Enitities;
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

        public Product GetOrCreateAndGet(string name)
        {
            var product = GetProductByName(name);
            if (product != null) return product;
            product = new Product
            {
                Category = 1,
                Name = TitledString(name)
            };
            _productRepository.AddProduct(product);
            return product.Id <= 0 ? null : product;
        }

        public Product GetProductByName(string name) => 
            _productRepository.GetProductByName(name);

    }
}
