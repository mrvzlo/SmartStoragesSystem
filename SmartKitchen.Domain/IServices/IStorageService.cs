using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        List<StorageDisplayModel> GetStoragesWithDescriptionByOwnerEmail(string email);
        void DeleteStorageById(int id, string email);
        StorageDisplayModel GetStorageDescriptionById(int id, string email);
        ItemCreationResponse AddStorage(StorageCreationModel model, string email);
        bool UpdateStorageName(string name, int id, string email);
    }
}
