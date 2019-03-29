// ReSharper disable InconsistentNaming
using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum Weight
    {
        [Description("kg")]
        Kilogram = 0,
        [Description("lbs")]
        Pound = 1
    }
}
