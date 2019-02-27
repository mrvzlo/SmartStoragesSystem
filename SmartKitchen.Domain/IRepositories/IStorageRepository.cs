﻿using System.Linq;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepositories
{
    public interface IStorageRepository
    {
        Storage GetStorageById(int id);
        Storage GetStorageByNameAndOwner(string name, int owner);
        void AddStorage(Storage storage);
        void DeleteStorageById(int id);
        void ReplaceType(int fromId, int toId);
    }
}
