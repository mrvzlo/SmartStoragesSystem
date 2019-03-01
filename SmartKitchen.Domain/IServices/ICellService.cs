using System;
using System.Collections.Generic;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrAddAndGet(CellCreationModel model, string email);
        ItemCreationResponse AddCell(CellCreationModel model, string email);
        CellDisplayModel GetCellDisplayModelById(int id, string email);
        ServiceResponse DeleteCellById(int id, string email);
        List<CellDisplayModel> GetCellsOfStorage(int storageId, string email);
        CellDisplayModel UpdateCellBestBefore(int id, DateTime? value, string email);
        CellDisplayModel UpdateCellAmount(int id, int value, string email);
    }
}
