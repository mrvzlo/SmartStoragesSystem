using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IAuthenticationService
    {
        Person GetPersonByEmail(string email);
        AuthenticationResponse SignIn(SignInModel model);
        AuthenticationResponse SignUp(SignUpModel model);
    }
}
