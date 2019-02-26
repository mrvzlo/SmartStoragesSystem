﻿using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System.Linq;
using System.Web.Helpers;

namespace SmartKitchen.DomainService.Services
{
    class AuthenticationService : IAuthenticationService
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

        public Person GetPersonByEmail(string email)
        {
            return _personRepository.GetPersonByEmail(email);
        }

        public AuthenticationResponse SignIn(SignInModel model)
        {
            var response = new AuthenticationResponse();
            var person = GetPersonByEmail(model.Email);

            if (person == null || !Crypto.VerifyHashedPassword(person.Password, model.Password))
                response.Errors.Add(new ModelStateError("", AuthenticationError.EmailOrPasswordIsIncorrect));
            else
            {
                response.Email = person.Email;
                response.Role = person.Role;
            }
            return response;
        }
        public AuthenticationResponse SignUp(SignUpModel model)
        {
            var response = new AuthenticationResponse();
            var personByEmail = _personRepository.GetPersonByName(model.Username);
            var personByName = GetPersonByEmail(model.Email);
            if (personByEmail != null)
                response.Errors.Add(new ModelStateError(nameof(model.Email), AuthenticationError.ThisEmailIsTaken));
            if (personByName != null)
                response.Errors.Add(new ModelStateError(nameof(model.Username), AuthenticationError.ThisNameIsTaken));
            if (response.Errors.Any()) return response;
            var person = new Person
            {
                Name = model.Username,
                Password = Crypto.HashPassword(model.Password),
                Email = model.Email
            };
            _personRepository.Register(person);

            if (person.Id <= 0)
                response.Errors.Add(new ModelStateError("", AuthenticationError.AnErrorOccured));
            else
            {
                CreateInitialStorage(person.Id);
                response.Email = person.Email;
                response.Role = person.Role;
            }
            return response;
        }

        public void CreateInitialStorage(int personId)
        {
            var firstType = _storageTypeRepository.GetAllStorageTypes().First();
            _storageRepository.AddStorage(new Storage
            {
                Name = firstType.Name,
                Owner = personId,
                TypeId = firstType.Id
            });
        }
    }
}
