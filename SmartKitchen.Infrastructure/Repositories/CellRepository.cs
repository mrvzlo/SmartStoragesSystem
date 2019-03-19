using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppDbContext _dbContext;

        public CellRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        public IQueryable<Cell> GetCellsForStorage(int storageId) =>
            _dbContext.Cells.Where(x => x.StorageId == storageId);

        public void AddOrUpdateCell(Cell cell) =>
            _dbContext.InsertOrUpdate(cell);

        public Cell GetCellByProductAndStorage(int product, int storage) =>
            _dbContext.Cells.FirstOrDefault(x => x.ProductId == product && x.StorageId == storage);

        public Cell GetCellById(int id) =>
            _dbContext.Cells.Find(id);

        public void DeleteCell(Cell cell) =>
            _dbContext.Delete(cell);

        public void DeleteCellAmountChanges(ICollection<CellChange> cellChanges) =>
            _dbContext.CellChanges.RemoveRange(cellChanges);

        public void AddCellAmountChange(CellChange change) =>
            _dbContext.InsertOrUpdate(change);
    }
}
