using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        ModelStateError StorageAccessError(Storage storage, Person person);
        ModelStateError BasketAccessError(Basket basket, Person person);
        ModelStateError StorageAccessError(Storage storage, string email);
        ModelStateError BasketAccessError(Basket basket, string email);
        Person GetPersonByEmail(string email);
        bool ExistsById(int id);
    }
}
