using System;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IPersonRepository
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByName(string name);
        Person GetPersonById(int id);
        void RegisterOrUpdate(Person person);
        Person GetPersonByToken(Guid token);
    }
}
