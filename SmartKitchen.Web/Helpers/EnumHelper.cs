using System;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Web.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<Currency> GetAllCurrencies() => Enum.GetValues(typeof(Currency)).Cast<Currency>();
        public static IEnumerable<Weight> GetAllWeights() => Enum.GetValues(typeof(Weight)).Cast<Weight>();
    }
}