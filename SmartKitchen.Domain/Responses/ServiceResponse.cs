using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKitchen.Domain.Responses
{
    public class ServiceResponse
    {
        public bool IsSuccessful { get; set; }
        public List<ModelStateError> Errors { get; set; }

        public ServiceResponse()
        {
            IsSuccessful = false;
            Errors = new List<ModelStateError>();
        }
    }
}
