using System;
using System.Collections.Generic;
using SmartKitchen.Domain.Enitities;
using System.Linq;

namespace SmartKitchen.Domain.IRepositories
{
    public interface ICellRepository
    {
        IQueryable<Cell> GetCellsForStorage(int storageId);
        void AddCell(Cell cell);
        Cell GetCellByProductAndStorage(int product, int storage);
        Cell GetCellById(int id);
        void DeleteCell(Cell cell);
        void UpdateDatetime(int cell, DateTime? datetime);
        void UpdateAmount(int cell, int amount);
        void DeleteCellsRange(ICollection<Cell> cells);
    }
}
