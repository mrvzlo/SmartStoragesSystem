using System;
using System.ComponentModel;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Enums.Atributes;

namespace SmartKitchen.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return GetDescription(value as object);
        }
        public static StatusType GetStatus(this Enum value)
        {
            return GetStatus(value as object);
        }

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

        public static StatusType GetStatus(object value)
        {
            if (value == null)
                return StatusType.Unknown;

            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return StatusType.Unknown;

            var attributes = (StatusAttribute[])fieldInfo.GetCustomAttributes(typeof(StatusAttribute), false);
            return attributes.Length > 0 ? attributes[0].Type : StatusType.Unknown;
        }
    }
}