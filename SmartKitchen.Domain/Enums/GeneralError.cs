using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum GeneralError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("This name is already taken")]
        NameIsAlreadyTaken = 2,
        [Description("Access to basket denied")]
        AccessToBasketDenied = 3,
        [Description("Access to storage denied")]
        AccessToStorageDenied = 4,
        [Description("Basket is closed")]
        BasketIsClosed = 5,
        [Description("Product is not bought")]
        ProductIsNotBought = 6,

        [Description("Storage was not found")]
        StorageWasNotFound = 7,
        [Description("Basket was not found")]
        BasketWasNotFound = 8,
        [Description("Cell was not found")]
        CellWasNotFound = 9,
        [Description("Basket product was not found")]
        BasketProductWasNotFound = 10,
        [Description("Person was not found")]
        PersonWasNotFound = 11,
        [Description("Storage type was not found")]
        StorageTypeWasNotFound = 12,
        [Description("Category was not found")]
        CategoryWasNotFound = 13,

        [Description("Cant Remove Primal Category")]
        CantRemovePrimalCategory = 14,
        [Description("Cant Replace To Itself")]
        CantReplaceToItself = 15,

        [Description("Unknown command")]
        UnknownCommand = 16,
        [Description("Value is negative")]
        NegativeNumber = 17,
    }
}
