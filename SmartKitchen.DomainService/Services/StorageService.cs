using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class StorageService : BaseService, IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;

        public StorageService(IStorageRepository storageRepository, IPersonRepository personRepository, IPersonService personService)
        {
            _storageRepository = storageRepository;
            _personRepository = personRepository;
            _personService = personService;
        }

        public List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return _storageRepository.GetAllUserStorages(person.Id).ProjectTo<StorageDescription>(MapperConfig).ToList();
        }
    }
}
