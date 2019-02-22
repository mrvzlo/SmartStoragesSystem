using System;
using System.ComponentModel;

namespace SmartKitchen.Helpers
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string GetDescription(this Enum value)
		{
			return GetDescription(value as object);
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string GetDescription(object value)
		{
			if (value == null)
				return "";

			var fieldInfo = value.GetType().GetField(value.ToString());
			if (fieldInfo == null)
				return "";

			var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attributes.Length > 0 ? attributes[0].Description : value.ToString();
		}
	}
}