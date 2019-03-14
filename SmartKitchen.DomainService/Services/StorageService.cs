using System;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace SmartKitchen.DomainService.Services
{
    public class StorageService : BaseService, IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageTypeService _storageTypeService;
        private readonly IPersonRepository _personRepository;
        private readonly ICellService _cellService;

        public StorageService(IStorageRepository storageRepository, IPersonRepository personRepository, IStorageTypeService storageTypeService, ICellService cellService)
        {
            _storageRepository = storageRepository;
            _personRepository = personRepository;
            _storageTypeService = storageTypeService;
            _cellService = cellService;
        }

        public IQueryable<StorageDisplayModel> GetStoragesWithDescriptionByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return person.Storages.AsQueryable().ProjectTo<StorageDisplayModel>(MapperConfig);
        }

        public void DeleteStorageById(int id, string email)
        {
            var person = _personRepository.GetPersonByEmail(email).Id;
            var storage = _storageRepository.GetStorageById(id);
            if (storage.PersonId != person) return;
            foreach (var c in storage.Cells) _cellService.DeleteCell(c);
            _storageRepository.DeleteStorage(storage);
        }

        public StorageDisplayModel GetStorageDescriptionById(int id, string email)
        {
            var storage = _storageRepository.GetStorageById(id);
            var personId = _personRepository.GetPersonByEmail(email).Id;
            return storage == null || storage.PersonId != personId ? null : Mapper.Map<StorageDisplayModel>(storage);
        }

        public ItemCreationResponse AddStorage(StorageCreationModel model, string email)
        {
            var response = new ItemCreationResponse();
            model.Name = model.Name.Trim();
            var person = _personRepository.GetPersonByEmail(email);
            var exists = person.Storages.Any(x => x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
            if (exists) response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
            if (!_storageTypeService.ExistsWithId(model.TypeId)) response.AddError(GeneralError.ItemNotFound, nameof(model.TypeId));
            if (!response.Successful()) return response;
            var storage = new Storage
            {
                Name = model.Name,
                PersonId = person.Id,
                TypeId = model.TypeId
            };
            _storageRepository.AddOrUpdateStorage(storage);
            response.AddedId = storage.Id;
            response.AddedGroupId = storage.TypeId;
            return response;
        }

        public bool UpdateStorageName(NameCreationModel model, int id, string email)
        {
            model.Name = model.Name.Trim();
            var storage = _storageRepository.GetStorageById(id);
            var personId = _personRepository.GetPersonByEmail(email).Id;
            if (storage.PersonId != personId || model.Name == storage.Name) return false;
            if (_storageRepository.GetStorageByNameAndOwner(model.Name, personId) != null) return false;
            storage.Name = model.Name;
            _storageRepository.AddOrUpdateStorage(storage);
            return true;
        }
    }
}
