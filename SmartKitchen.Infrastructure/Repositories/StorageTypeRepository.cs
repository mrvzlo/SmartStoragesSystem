using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;

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

        public StorageType GetStorageTypeByName(string name) =>
            _dbContext.StorageTypes.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public void AddStorageType(StorageType storageType) =>
            _dbContext.InsertOrUpdate(storageType);
    }
}
