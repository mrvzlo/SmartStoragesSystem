using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrAddAndGet(CellCreationModel model, string email);
        Cell GetCellByProductAndStorage(int product, int storage);
        ItemCreationResponse AddCell(CellCreationModel model, string email);
        CellDisplayModel GetCellDisplayModelById(int id, string email);
    }
}
