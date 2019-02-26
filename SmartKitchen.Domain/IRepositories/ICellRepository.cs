using SmartKitchen.Domain.Enitities;
using System.Linq;

namespace SmartKitchen.Domain.IRepositories
{
    public interface ICellRepository
    {
        IQueryable<Cell> GetCellsForStorage(int storageId);
        void AddCell(Cell cell);
    }
}
