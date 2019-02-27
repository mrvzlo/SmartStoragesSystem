using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        Person GetPersonByEmail(string email);
        bool ExistsById(int id);
    }
}
