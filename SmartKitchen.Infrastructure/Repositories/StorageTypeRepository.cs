using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class StorageTypeRepository : IStorageTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public StorageTypeRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public IQueryable<StorageType> GetAllStorageTypes() =>
            _dbContext.StorageTypes.AsQueryable();

        public StorageType GetStorageTypeById(int id) =>
            _dbContext.StorageTypes.Find(id);

        public void DeleteStorageTypeById(int id) =>
            _dbContext.Delete(_dbContext.StorageTypes.Find(id));
        
        public void AddOrUpdateStorageType(StorageType storageType) =>
            _dbContext.InsertOrUpdate(storageType);

        public bool NameIsUnique(string name, int id) =>
            !_dbContext.StorageTypes.Any(x => x.Id != id && x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
