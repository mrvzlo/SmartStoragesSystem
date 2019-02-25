using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class StorageTypeRepository : IStorageTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public StorageTypeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<StorageType> GetAllStorageTypes() =>
            _dbContext.StorageTypes;
    }
}
