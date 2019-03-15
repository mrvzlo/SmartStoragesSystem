using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IStorageRepository
    {
        Storage GetStorageById(int id);
        Storage GetStorageByNameAndOwner(string name, int owner);
        void AddOrUpdateStorage(Storage storage);
        void DeleteStorage(Storage storage);
        void ReplaceType(int fromId, int toId);
    }
}
