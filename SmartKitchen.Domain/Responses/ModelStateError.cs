using System;

namespace SmartKitchen.Domain.Responses
{
    public class ModelStateError
    {
        public string Key { get; set; }
        public Enum ErrorEnum { get; set; }

        public ModelStateError(string key, Enum errorEnum)
        {
            Key = key;
            ErrorEnum = errorEnum;
        }
    }
}
