using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Responses;
using System.Collections.Generic;

namespace SmartKitchen.Domain.IServices
{
    public interface ICategoryService
    {
        CategoryDisplay GetCategoryDisplayById(int id);
        List<CategoryDisplay> GetAllCategoryDisplays();
        ServiceResponse AddCategoryWithName(string name);
        void ReplaceCategory(int fromId, int toId);
    }
}
