﻿using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IService
{
    public interface IAuthenticationService
    {
        Person GetPersonByEmail(string email);
        AuthenticationResponse SignIn(SignInModel model);
        AuthenticationResponse SignUp(SignUpModel model);
    }
}
