using System.Collections.Generic;
using SmartKitchen.Domain.DisplayModel;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email);
    }
}
