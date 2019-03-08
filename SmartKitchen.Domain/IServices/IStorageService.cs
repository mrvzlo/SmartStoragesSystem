using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email);
        void DeleteStorageById(int id, string email);
        StorageDescription GetStorageDescriptionById(int id, string email);
        ItemCreationResponse AddStorage(StorageCreationModel model, string email);
    }
}
