using System.ComponentModel;
using SmartKitchen.Domain.Enums.Atributes;

namespace SmartKitchen.Domain.Enums
{
	public enum Safety
    {
        [Description("Is safe")]
        [Status(StatusType.Normal)]
        IsSafe = 0,

        [Description("Expires tomorrow")]
        [Status(StatusType.Warning)]
        ExpiresTomorrow = 1,

        [Description("Expires today")]
        [Status(StatusType.Warning)]
        ExpiresToday = 2,

        [Description("Expired")]
        [Status(StatusType.Danger)]
        Expired = 3,

        [Description("Unknown")]
        [Status(StatusType.Unknown)]
        Unknown = 4
	}
}