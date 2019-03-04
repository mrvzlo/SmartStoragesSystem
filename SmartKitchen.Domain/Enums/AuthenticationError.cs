using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
    public enum AuthenticationError
    {
        [Description("An error occured")]
        AnErrorOccured = 1,
        [Description("Email or password is incorrect")]
        EmailOrPasswordIsIncorrect = 2,
        [Description("This name is already registered")]
        ThisNameIsTaken = 3,
        [Description("This email is already registered")]
        ThisEmailIsTaken = 4
    }
}
