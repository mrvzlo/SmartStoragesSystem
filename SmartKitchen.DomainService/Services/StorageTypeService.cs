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

        /// <summary>
        /// If types exist update all storages first type id to second
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public bool ReplaceStorageType(int fromId, int toId)
        {
            var fromType = _storageTypeRepository.GetStorageTypeById(fromId);
            var toType = _storageTypeRepository.GetStorageTypeById(toId);
            if (fromType == null || toType == null || fromId == toId) return false;
            _storageRepository.ReplaceType(fromId, toId);
            _storageTypeRepository.DeleteStorageTypeById(fromId);
            return true;
        }

        /// <summary>
        /// Replace type if existing id is passed else create the new one
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ItemCreationResponse AddOrUpdateStorageType(StorageTypeCreationModel model)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            if (!_storageTypeRepository.NameIsUnique(model.Name, model.Id))
                return response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));

            var oldType = _storageTypeRepository.GetStorageTypeById(model.Id);
            if (oldType != null)
            {
                response.AddedId = oldType.Id;
                if (oldType.Name == model.Name && oldType.Background == model.Background) return response;
                oldType.Name = model.Name;
                oldType.Background = model.Background;
                _storageTypeRepository.AddOrUpdateStorageType(oldType);
            }
            else
            {
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
