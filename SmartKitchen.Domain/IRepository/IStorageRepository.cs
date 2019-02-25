using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface IStorageRepository
    {
        Storage GetStorageById(int id);
        IQueryable<Storage> GetAllUserStorages(int person);
    }
}
