using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IPersonRepository
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByName(string name);
        void Register(Person person);
    }
}
