using System.Collections.Generic;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        ServiceResponse IsOwner(Storage s, Person p);
        ServiceResponse IsOwner(Basket b, Person p);
    }
}
