using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.DisplayModel;

namespace SmartKitchen.Domain.IService
{
    public interface IHomeService
    {
        HelpModel GetHelpModel();
    }
}
