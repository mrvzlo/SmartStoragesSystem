using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;
using System;
using System.Linq;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrAddAndGet(CellCreationModel model, string email);
        ItemCreationResponse AddCell(CellCreationModel model, string email);
        CellDisplayModel GetCellDisplayModelById(int id, string email);
        ServiceResponse DeleteCellById(int id, string email);
        IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email);
        CellDisplayModel UpdateCellBestBefore(int id, DateTime? value, string email);
        CellDisplayModel UpdateCellAmount(int id, Amount value, string email);
    }
}
