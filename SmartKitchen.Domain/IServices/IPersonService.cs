using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        Person GetPersonByEmail(string email);
        void UpdateKeyPair(string email);
        ServiceResponse Interpretator(string request);
    }
}
