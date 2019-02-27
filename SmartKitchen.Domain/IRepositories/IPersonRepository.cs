using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IPersonRepository
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByName(string name);
        Person GetPersonById(int id);
        void Register(Person person);
    }
}
