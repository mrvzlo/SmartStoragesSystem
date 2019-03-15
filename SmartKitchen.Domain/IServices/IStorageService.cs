using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Linq;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        IQueryable<StorageDisplayModel> GetStoragesWithDescriptionByOwnerEmail(string email);
        void DeleteStorageById(int id, string email);
        StorageDisplayModel GetStorageDescriptionById(int id, string email);
        ItemCreationResponse AddStorage(StorageCreationModel model, string email);
        bool UpdateStorageName(NameCreationModel name, int id, string email);
    }
}
