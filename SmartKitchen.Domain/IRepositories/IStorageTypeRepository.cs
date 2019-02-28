using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IStorageTypeRepository
    {
        IQueryable<StorageType> GetAllStorageTypes();
        StorageType GetStorageTypeById(int id);
        void DeleteStorageTypeById(int id);
        StorageType GetStorageTypeByName(string name);
        void AddOrUpdateStorageType(StorageType storageType);
        bool ExistsWithName(string name);
    }
}
