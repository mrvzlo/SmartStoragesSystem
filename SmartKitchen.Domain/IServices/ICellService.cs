using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrCreateAndGet(CellCreationModel model);
        Cell GetCellByProductAndStorage(int product, int storage);
    }
}
