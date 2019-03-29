using SmartKitchen.Domain.Enitities;
using System.Collections.Generic;
using System.Linq;

namespace SmartKitchen.Domain.IRepositories
{
    public interface ICellRepository
    {
        IQueryable<Cell> GetCellsForStorage(int storageId);
        void AddOrUpdateCell(Cell cell);
        void AddCellAmountChange(CellChange change);
        Cell GetCellByProductAndStorage(int product, int storage);
        Cell GetCellById(int id);
        void DeleteCell(Cell cell);
        void DeleteCellAmountChanges(ICollection<CellChange> cellChanges);
    }
}
