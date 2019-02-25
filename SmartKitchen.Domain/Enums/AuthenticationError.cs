using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum AuthenticationError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("User not found")]
        UserNotFound = 2,
        [Description("Email or password is incorrect")]
        EmailOrPasswordIsIncorrect = 3,
        [Description("This name is already registered")]
        ThisNameIsTaken = 4,
        [Description("This emial is already registered")]
        ThisEmailIsTaken = 5
    }
}
