﻿using SmartKitchen.Domain.Enums;

namespace SmartKitchen.Domain.Responses
{
    public class AuthenticationResponse : ServiceResponse
    {
        public string Email;
        public Role Role;
    }
}
