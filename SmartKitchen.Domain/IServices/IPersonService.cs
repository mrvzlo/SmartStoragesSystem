using System;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByToken(Guid token);
        void UpdateToken(string email);
    }
}
