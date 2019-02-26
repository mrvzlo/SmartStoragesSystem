using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    public class StorageService : BaseService, IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IPersonRepository _personRepository;

        public StorageService(IStorageRepository storageRepository, IPersonRepository personRepository)
        {
            _storageRepository = storageRepository;
            _personRepository = personRepository;
        }

        public List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return _storageRepository.GetAllUserStorages(person.Id).ProjectTo<StorageDescription>(MapperConfig).ToList();
        }
    }
}
