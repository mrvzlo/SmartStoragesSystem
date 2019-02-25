using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IBasketRepository
    {
        IQueryable<Basket> GetAllUserBaskets(int personId);
        Basket GetBasketByNameAndOwner(string name, int owner);
        void AddBasket(Basket basket);
    }
}
