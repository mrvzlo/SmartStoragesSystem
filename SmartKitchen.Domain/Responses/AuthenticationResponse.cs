using SmartKitchen.Domain.Enitities;
using SmartKitchen.Enums;

namespace SmartKitchen.Domain.Responses
{
    public class AuthenticationResponse : ServiceResponse
    {
        public string Email;
        public Role Role;
    }
}
