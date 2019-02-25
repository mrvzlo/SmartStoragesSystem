using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKitchen.Domain.Enums
{
    public enum AccessError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("You don't have permission")]
        NoPermission = 2,
        [Description("Storage not found")]
        StorageNotFound = 3,
        [Description("Basket not found")]
        BasketNotFound = 4
    }
}
