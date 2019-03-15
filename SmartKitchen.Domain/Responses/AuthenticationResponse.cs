using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.Responses
{
    public class AuthenticationResponse : ServiceResponse
    {
        public string Email;
        public Role Role;

        public AuthenticationResponse() { }

        public AuthenticationResponse(ServiceResponse response)
        {
            Role = Role.Simple;
            Email = "";
            Errors = response.Errors;
        }
    }
}
