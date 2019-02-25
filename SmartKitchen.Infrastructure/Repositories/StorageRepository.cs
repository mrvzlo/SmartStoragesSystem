using SmartKitchen.Domain.Enitities;
using System.Collections.Generic;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class StorageRepository
    {
        private readonly AppDbContext _dbContext;

        public StorageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Storage GetStorageById(int id)
        {
            return _dbContext.Storages.Find(id);
        }

        public List<Storage> GetAllUserStorages(int person)
        {
            return _dbContext.Storages.Where(x => x.Owner == person).ToList();
        }
    }
}
