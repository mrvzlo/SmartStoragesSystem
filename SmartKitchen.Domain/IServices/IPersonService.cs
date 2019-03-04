using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        Person GetPersonByEmail(string email);
        bool ExistsById(int id);
    }
}
