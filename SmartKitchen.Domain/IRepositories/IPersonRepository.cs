using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IPersonRepository
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByName(string name);
        void RegisterOrUpdate(Person person);
        Person GetPersonById(int id);
    }
}
