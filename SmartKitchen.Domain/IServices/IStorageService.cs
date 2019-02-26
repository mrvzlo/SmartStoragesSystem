using SmartKitchen.Domain.DisplayModels;
using System.Collections.Generic;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IStorageService
    {
        List<StorageDescription> GetStoragesWithDescriptionByOwnerEmail(string email);
    }
}
