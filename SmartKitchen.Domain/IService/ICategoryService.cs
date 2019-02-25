using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Responses;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IService
{
    public interface ICategoryService
    {
        CategoryDisplay GetCategoryDisplayById(int id);
        List<CategoryDisplay> GetAllCategoryDisplays();
        ServiceResponse AddCategoryWithName(string name);
        void ReplaceCategory(int fromId, int toId);
    }
}
