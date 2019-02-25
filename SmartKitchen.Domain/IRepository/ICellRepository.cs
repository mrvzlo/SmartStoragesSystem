using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface ICellRepository
    {
        IQueryable<Cell> GetCellsForStorage(int storageId);
    }
}
