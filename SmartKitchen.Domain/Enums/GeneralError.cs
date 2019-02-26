using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKitchen.Domain.Enums
{
    public enum GeneralError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("This name is already taken")]
        NameIsAlreadyTaken = 2
    }
}
