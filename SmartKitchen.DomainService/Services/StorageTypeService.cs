using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Linq;
using SmartKitchen.Domain.CreationModels;

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

        public bool ReplaceType(int fromId, int toId)
        {
            var fromType = _storageTypeRepository.GetStorageTypeById(fromId);
            var toType = _storageTypeRepository.GetStorageTypeById(toId);
            if (fromType == null || toType == null || fromId == 1) return false;
            _storageRepository.ReplaceType(fromId, toId);
            _storageTypeRepository.DeleteStorageTypeById(fromId);
            return true;
        }

        public ItemCreationResponse AddOrUpdateStorageType(StorageTypeCreationModel model)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            var oldType = _storageTypeRepository.GetStorageTypeById(model.Id);
            if (oldType != null)
            {
                oldType.Name = model.Name;
                oldType.Background = model.Background;
                _storageTypeRepository.AddOrUpdateStorageType(oldType);
                response.AddedId = oldType.Id;
            }
            else
            {
                if (_storageTypeRepository.ExistsWithName(model.Name))
                {
                    response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
                    return response;
                }

                var newType = new StorageType
                {
                    Background = model.Background,
                    Name = model.Name
                };
                _storageTypeRepository.AddOrUpdateStorageType(newType);
                response.AddedId = newType.Id;
            }

            return response;
        }
    }
}
