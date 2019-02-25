using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKitchen.Domain.Responses
{
    public class ModelStateError
    {
        public string Key;
        public Enum ErrorEnum;

        public ModelStateError(string key, Enum errorEnum)
        {
            Key = key;
            ErrorEnum = errorEnum;
        }
    }
}
