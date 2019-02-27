using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageTypeService
    {
        IQueryable<StorageType> GetAllStorageTypes();
        bool ExistsWithId(int id);
    }
}
