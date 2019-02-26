using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        ServiceResponse IsOwner(Storage s, Person p);
        ServiceResponse IsOwner(Basket b, Person p);
    }
}
