using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum GeneralError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("This name is already taken")]
        NameIsAlreadyTaken = 2,
        [Description("Access denied")]
        AccessDenied = 3,
        [Description("Item not found")]
        ItemNotFound = 4,
        [Description("Basket is closed")]
        BasketIsClose = 5,
        [Description("Product is not bought")]
        ProductIsNotBought = 6,
    }
}
