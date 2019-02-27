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
        Basket GetBasketById(int id);
        Basket GetBasketByNameAndOwner(string name, int owner);
        void AddBasket(Basket basket);
        void LockBasketById(int id);
        void DeleteBasket(Basket basket);
    }
}
