using System.ComponentModel;

namespace SmartKitchen.Domain.Enums
{
	public enum Role
	{
        [Description("Simple")]
		Simple = 0,
        [Description("Admin")]
        Admin = 1
	}
}