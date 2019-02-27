using System;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using System.Linq;
using Z.EntityFramework.Plus;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly AppDbContext _dbContext;

        public CellRepository(AppDbContext dbContext) => 
            _dbContext = dbContext;

        public IQueryable<Cell> GetCellsForStorage(int storageId) =>
            _dbContext.Cells.Where(x => x.StorageId == storageId);

        public void AddCell(Cell cell) =>
            _dbContext.InsertOrUpdate(cell);

        public Cell GetCellByProductAndStorage(int product, int storage) =>
            _dbContext.Cells.FirstOrDefault(x => x.ProductId == product && x.StorageId == storage);

        public Cell GetCellById(int id) =>
            _dbContext.Cells.Find(id);

        public void DeleteCell(Cell cell) =>
            _dbContext.Delete(cell);

        public void UpdateAmount(int cell, int amount) =>
            _dbContext.Cells.Where(x => x.Id == cell).Update(x => new Cell { Amount = amount });

        public void UpdateDatetime(int cell, DateTime? datetime) =>
            _dbContext.Cells.Where(x => x.Id == cell).Update(x => new Cell { BestBefore = datetime });
    }
}
