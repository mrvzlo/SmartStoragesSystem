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

        public ModelStateError StorageAccessError(Storage storage, Person person) =>
            storage == null || person == null 
                ? new ModelStateError("", GeneralError.ItemNotFound) 
                : storage.Owner != person.Id 
                    ? new ModelStateError("", GeneralError.AccessDenied) 
                    : null;

        public ModelStateError BasketAccessError(Basket basket, Person person) =>
            basket == null || person == null 
                ? new ModelStateError("", GeneralError.ItemNotFound) 
                : basket.Owner != person.Id 
                    ? new ModelStateError("", GeneralError.AccessDenied) 
                    : null;

        public ModelStateError StorageAccessError(Storage storage, string email)
        {
            var person = GetPersonByEmail(email);
            return StorageAccessError(storage, person);
        }

        public ModelStateError BasketAccessError(Basket basket, string email)
        {
            var person = GetPersonByEmail(email);
            return BasketAccessError(basket, person);
        }

        public Person GetPersonByEmail(string email) =>
            _personRepository.GetPersonByEmail(email);

        public bool ExistsById(int id) => 
            _personRepository.GetPersonById(id) != null;
    }
}
