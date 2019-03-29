// ReSharper disable InconsistentNaming
using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum Currency
    {
        [Description("EUR")]
        EUR = 0,
        [Description("USD")]
        USD = 1,
        [Description("GBP")]
        GBP = 2,
        [Description("JPY")]
        JPY = 3,
        [Description("RUB")]
        RUB = 4
    }
}
