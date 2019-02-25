using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IServices
{
    public interface ICategoryService
    {
        CategoryDisplay GetCategoryDisplayById(int id);
    }
}
