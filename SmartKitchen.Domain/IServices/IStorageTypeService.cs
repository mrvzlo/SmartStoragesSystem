using System.Linq;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageTypeService
    {
        IQueryable<StorageType> GetAllStorageTypes();
        bool ExistsWithId(int id);
        bool ReplaceType(int fromId, int toId);
        ItemCreationResponse AddOrUpdateStorageType(StorageTypeCreationModel model);
    }
}
