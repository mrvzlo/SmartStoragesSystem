using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using SmartKitchen.Models;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    public class StorageTypeService : BaseService, IStorageTypeService
    {
        private readonly IStorageTypeRepository _storageTypeRepository;
        private readonly IStorageRepository _storageRepository;

        public StorageTypeService(IStorageTypeRepository storageTypeRepository, IStorageRepository storageRepository)
        {
            _storageTypeRepository = storageTypeRepository;
            _storageRepository = storageRepository;
        }

        public IQueryable<StorageType> GetAllStorageTypes() =>
            _storageTypeRepository.GetAllStorageTypes();

        public bool ExistsWithId(int id) =>
            _storageTypeRepository.GetStorageTypeById(id) != null;

        public void ReplaceType(int fromId, int toId)
        {
            var fromType = _storageTypeRepository.GetStorageTypeById(fromId);
            var toType = _storageTypeRepository.GetStorageTypeById(toId);
            if (fromType == null || toType == null || fromId == 1) return;
            _storageRepository.ReplaceType(fromId, toId);
            _storageTypeRepository.DeleteStorageTypeById(fromId);
        }

        public ItemCreationResponse AddOrUpdateStorageType(StorageTypeCreationModel model)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            var exists = _storageTypeRepository.GetStorageTypeByName(model.Name) != null;
            if (exists)
            {
                response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
                return response;
            }

            var storageType = new StorageType
            {
                Background = model.Background,
                Name = model.Name
            };

            if (model.Id > 0) storageType.Id = model.Id;
            _storageTypeRepository.AddStorageType(storageType);
            response.AddedId = model.Id;
            return response;
        }
    }
}
