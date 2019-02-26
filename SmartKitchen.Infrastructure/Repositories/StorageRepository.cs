using SmartKitchen.Domain.Enitities;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly AppDbContext _dbContext;

        public StorageRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public void AddStorage(Storage storage) =>
            _dbContext.InsertOrUpdate(storage);

        public Storage GetStorageById(int id) => 
            _dbContext.Storages.Find(id);

        public IQueryable<Storage> GetAllUserStorages(int person) =>
            _dbContext.Storages.Where(x => x.Owner == person);
    }
}
