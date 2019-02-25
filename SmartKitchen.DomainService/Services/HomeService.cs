using System.Linq;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;

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
