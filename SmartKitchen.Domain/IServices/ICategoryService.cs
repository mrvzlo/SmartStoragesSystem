using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Linq;
using SmartKitchen.Domain.CreationModels;

namespace SmartKitchen.Domain.IServices
{
    public interface ICategoryService
    {
        IQueryable<CategoryDisplayModel> GetAllCategoryDisplays();
        ServiceResponse AddCategory(NameCreationModel model);
        bool ReplaceCategory(int fromId, int toId);
    }
}
