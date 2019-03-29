using System;
using System.Collections.Generic;

namespace SmartKitchen.Domain.DisplayModels
{
    public class CryptRequest
    {
        public string Action { get; }
        public int Object { get; }
        public string Value { get; }

        public CryptRequest(IReadOnlyList<string> details)
        {
            Action = details[0];
            Object = Convert.ToInt32(details[1]);
            if (details.Count < 3) return;
            Value = details[2];
        }
    }
}