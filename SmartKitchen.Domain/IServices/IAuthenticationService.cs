using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IAuthenticationService
    {
        AuthenticationResponse SignIn(SignInModel model);
        AuthenticationResponse SignUp(SignUpModel model);
    }
}
