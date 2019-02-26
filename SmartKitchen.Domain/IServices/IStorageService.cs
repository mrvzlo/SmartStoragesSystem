using SmartKitchen.Domain.DisplayModels;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email);
    }
}
