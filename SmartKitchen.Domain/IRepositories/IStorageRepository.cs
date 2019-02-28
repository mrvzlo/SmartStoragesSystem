using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IStorageRepository
    {
        Storage GetStorageById(int id);
        Storage GetStorageByNameAndOwner(string name, int owner);
        void AddStorage(Storage storage);
        void DeleteStorage(Storage storage);
        void ReplaceType(int fromId, int toId);
    }
}
