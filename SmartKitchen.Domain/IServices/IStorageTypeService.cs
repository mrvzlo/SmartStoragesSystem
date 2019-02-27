using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageTypeService
    {
        IQueryable<StorageType> GetAllStorageTypes();
        bool ExistsWithId(int id);
        void ReplaceType(int fromId, int toId);
        ItemCreationResponse AddOrUpdateStorageType(StorageTypeCreationModel model);
    }
}
