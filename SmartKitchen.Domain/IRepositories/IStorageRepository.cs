using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IStorageRepository
    {
        Storage GetStorageById(int id);
        IQueryable<Storage> GetAllUserStorages(int person);
        void AddStorage(Storage storage);
    }
}
