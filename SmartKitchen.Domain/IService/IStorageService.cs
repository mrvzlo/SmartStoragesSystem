using System.Collections.Generic;
using SmartKitchen.Domain.DisplayModel;

namespace SmartKitchen.Domain.IService
{
    public interface IStorageService
    {
        List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email);
    }
}
