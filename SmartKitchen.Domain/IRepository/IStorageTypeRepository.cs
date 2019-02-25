using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface IStorageTypeRepository
    {
        IQueryable<StorageType> GetAllStorageTypes();
    }
}
