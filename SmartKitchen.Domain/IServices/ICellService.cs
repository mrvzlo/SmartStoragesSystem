using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;
using System;
using System.Linq;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrAddAndGet(CellCreationModel model, string email);
        ItemCreationResponse AddOrUpdateCell(CellCreationModel model, string email);
        CellDisplayModel GetCellDisplayModelById(int id, string email);
        ServiceResponse DeleteCellByIdAndEmail(int id, string email);
        void DeleteCell(Cell cell);
        IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email);
        ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email);
        ServiceResponse UpdateCellAmount(int id, int value, string email);
        ItemCreationResponse MoveProductToStorage(BasketProduct basketProduct, Basket basket, Person person);
    }
}
