using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.DisplayModel;

namespace SmartKitchen.Domain.IServices
{
    public interface IHomeService
    {
        HelpModel GetHelpModel();
    }
}
