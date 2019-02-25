using System.Collections.Generic;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IService
{
    public interface IPersonService
    {
        Response IsOwner(Storage s, Person p);
        Response IsOwner(Basket b, Person p);
    }
}
