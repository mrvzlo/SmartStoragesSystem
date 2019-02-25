using System.Collections.Generic;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IServices
{
    public interface IPersonService
    {
        Response IsOwner(Storage s, Person p);
        Response IsOwner(Basket b, Person p);
    }
}
