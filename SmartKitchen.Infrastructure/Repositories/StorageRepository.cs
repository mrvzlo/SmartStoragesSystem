using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly AppDbContext _dbContext;

        public StorageRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public void AddOrUpdateStorage(Storage storage) =>
            _dbContext.InsertOrUpdate(storage);

        public Storage GetStorageById(int id) =>
            _dbContext.Storages.Find(id);

        public void DeleteStorage(Storage storage) =>
            _dbContext.Delete(storage);

        public Storage GetStorageByNameAndOwner(string name, int owner) =>
            _dbContext.Storages.FirstOrDefault(x => x.PersonId == owner && x.Name.Equals(name));

        public void ReplaceType(int fromId, int toId) =>
            _dbContext.Storages.Where(x => x.TypeId == fromId).Update(x => new Storage { TypeId = toId });
    }
}
