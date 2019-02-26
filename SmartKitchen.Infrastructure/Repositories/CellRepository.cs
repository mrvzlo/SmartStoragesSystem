using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppDbContext _dbContext;

        public CellRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Cell> GetCellsForStorage(int storageId) =>
            _dbContext.Cells.Where(x => x.Storage == storageId);
    }
}
