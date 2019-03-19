using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Extensions;

namespace SmartKitchen.Web.Helpers
{
    public static class CookieHelper
    {
        public static HttpCookie GetCookie(HttpContextBase context, Cookie type)
        {
            if (CookieExists(context, type)) return context.Request.Cookies[type.GetDescription()];
            switch (type)
            {
                case Cookie.Currency:
                    return UpdateCookie(context, type, (Currency)0);
                case Cookie.Weight:
                    return UpdateCookie(context, type, (Weight)0);
                default: return null;
            }
        }

        public static string GetCurrency(HttpContextBase context) => 
            GetCurrencyByCode(GetCookie(context, Cookie.Currency).Value);

        private static bool CookieExists(HttpContextBase context, Cookie type) =>
            context.Request.Cookies.AllKeys.Contains(type.GetDescription());

        public static HttpCookie UpdateCookie(HttpContextBase context, Cookie type, Enum value)
        {
            var cookie = new HttpCookie(type.GetDescription(), value.GetDescription()) {Expires = DateTime.Now.AddYears(1)};
            context.Response.Cookies.Add(cookie);
            return cookie;
        }

        private static readonly Dictionary<string, string> Currencies = new Dictionary<string, string>
        {
            {"EUR", "€"},
            {"USD", "$"},
            {"GBP", "£"},
            {"JPY", "¥"},
            {"RUB", "₽"},
        };

        private static string GetCurrencyByCode(string code) =>
            Currencies.ContainsKey(code) ? Currencies[code] : null;
    }
}