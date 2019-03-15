using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System;

namespace SmartKitchen.DomainService.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Person GetPersonByEmail(string email) =>
            _personRepository.GetPersonByEmail(email);

        public Person GetPersonByToken(Guid token) =>
            _personRepository.GetPersonByToken(token);

        public void UpdateToken(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            if (person == null) return;
            person.Token = Guid.NewGuid();
            _personRepository.RegisterOrUpdate(person);
        }
    }
}
