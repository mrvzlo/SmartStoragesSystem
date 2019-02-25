using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Models;

namespace SmartKitchen.Domain.IService
{
    public interface ICategoryService
    {
        CategoryDisplay GetCategoryDisplayById(int id);
    }
}
