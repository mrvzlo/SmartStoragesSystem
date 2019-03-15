using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    public class HomeService : BaseService, IHomeService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStorageTypeRepository _storageTypeRepository;

        public HomeService(IProductRepository productRepository, IStorageTypeRepository storageTypeRepository)
        {
            _productRepository = productRepository;
            _storageTypeRepository = storageTypeRepository;
        }

        public HelpModel GetHelpModel()
        {
            return new HelpModel
            {
                ProductCount = _productRepository.GetAllProducts().Count(),
                StorageTypesCount = _storageTypeRepository.GetAllStorageTypes().Count()
            };
        }
    }
}
