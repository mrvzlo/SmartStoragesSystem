using System.ComponentModel;
using SmartKitchen.Domain.Enums.Atributes;

namespace SmartKitchen.Domain.Enums
{
	public enum Amount
    {
        [Description("None")]
        [Status(StatusType.Unknown)]
        None = 0,

        [Description("Lack")]
        [Status(StatusType.Warning)]
        Lack = 1,

        [Description("Plenty")]
        [Status(StatusType.Normal)]
        Plenty = 2
	}
}