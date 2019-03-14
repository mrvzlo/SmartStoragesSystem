﻿using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System;
using System.Linq;
using System.Web.Helpers;

namespace SmartKitchen.DomainService.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageTypeRepository _storageTypeRepository;

        public AuthenticationService(IPersonRepository personRepository, IStorageRepository storageRepository, IStorageTypeRepository storageTypeRepository)
        {
            _personRepository = personRepository;
            _storageRepository = storageRepository;
            _storageTypeRepository = storageTypeRepository;
        }

        public Person GetPersonByEmail(string email) =>
            _personRepository.GetPersonByEmail(email);

        public AuthenticationResponse SignIn(SignInModel model)
        {
            var response = new AuthenticationResponse();
            var person = GetPersonByEmail(model.Email);

            if (person == null || !Crypto.VerifyHashedPassword(person.Password, model.Password))
                return new AuthenticationResponse(response.AddError(AuthenticationError.EmailOrPasswordIsIncorrect));
            response.Email = person.Email;
            response.Role = person.Role;
            return response;
        }
        public AuthenticationResponse SignUp(SignUpModel model)
        {
            var response = new AuthenticationResponse();
            var personByEmail = _personRepository.GetPersonByName(model.Username);
            var personByName = GetPersonByEmail(model.Email);
            if (personByEmail != null)
                return new AuthenticationResponse(response.AddError(AuthenticationError.ThisEmailIsTaken, nameof(model.Email)));
            if (personByName != null)
                return new AuthenticationResponse(response.AddError(AuthenticationError.ThisEmailIsTaken, nameof(model.Username)));
            var person = new Person
            {
                Name = model.Username,
                Password = Crypto.HashPassword(model.Password),
                Email = model.Email,
                Token = Guid.NewGuid()
            };
            _personRepository.RegisterOrUpdate(person);

            CreateInitialStorage(person.Id);
            response.Email = person.Email;
            response.Role = person.Role;
            return response;
        }

        private void CreateInitialStorage(int personId)
        {
            var firstType = _storageTypeRepository.GetAllStorageTypes().First();
            var initialStorage = new Storage
            {
                Name = firstType.Name,
                PersonId = personId,
                TypeId = firstType.Id
            };
            _storageRepository.AddOrUpdateStorage(initialStorage);
        }
    }
}
