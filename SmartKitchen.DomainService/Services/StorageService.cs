using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.DomainService.Services
{
    public class StorageService : BaseService, IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageTypeService _storageTypeService;
        private readonly IPersonService _personService;

        public StorageService(IStorageRepository storageRepository, IPersonService personService, IStorageTypeService storageTypeService)
        {
            _storageRepository = storageRepository;
            _personService = personService;
            _storageTypeService = storageTypeService;
        }

        public List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email)
        {
            var person = _personService.GetPersonByEmail(email);
            return Mapper.Map<List<StorageDescription>>(person.Storages);
        }

        public void DeleteStorageById(int id) =>
            _storageRepository.DeleteStorageById(id);

        public StorageDescription GetStorageDescriptionById(int id, string email)
        {
            var storage = _storageRepository.GetStorageById(id);
            return storage == null || storage.Person.Email != email ? null : Mapper.Map<StorageDescription>(storage);
        }

        public ItemCreationResponse AddStorage(StorageCreationModel model, string email)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            var personId = _personService.GetPersonByEmail(email).Id;
            var exists = _storageRepository.GetStorageByNameAndOwner(model.Name, personId) != null;
            if (exists) response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
            if (!_storageTypeService.ExistsWithId(model.TypeId)) response.AddError(GeneralError.ItemNotFound, nameof(model.TypeId));
            if (!response.Successful()) return response;
            var storage = new Storage
            {
                Name = model.Name,
                Owner = personId,
                TypeId = model.TypeId
            };
            response.AddedId = storage.Id;
            response.AddedGroupId = storage.TypeId;
            return response;
        }
    }
}
