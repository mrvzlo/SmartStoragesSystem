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
        void AddOrUpdateBasket(Basket basket);
        void DeleteBasket(Basket basket);
    }
}
