using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;

namespace SmartKitchen.DomainService
{
    public class StorageSevice : IStorageService
    {
        private readonly IStorageRepository _storageRepository;

        public StorageSevice(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public StorageDescription GetDescriptionForStorage(Storage storage)
        {

        }
    }
}
