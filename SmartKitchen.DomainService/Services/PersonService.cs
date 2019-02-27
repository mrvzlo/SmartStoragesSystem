using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IStorageRepository _storageRepository;

        public PersonService(IPersonRepository personRepository, IStorageRepository storageRepository)
        {
            _personRepository = personRepository;
            _storageRepository = storageRepository;
        }

        public Person GetPersonByEmail(string email) =>
            _personRepository.GetPersonByEmail(email);

        public bool ExistsById(int id) => 
            _personRepository.GetPersonById(id) != null;
    }
}
