using System;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
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

        /// <summary>
        /// Sign person in if he exists and password match
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthenticationResponse SignIn(SignInModel model)
        {
            var response = new AuthenticationResponse();
            var person = _personRepository.GetPersonByName(model.Username);

            if (person == null || !Crypto.VerifyHashedPassword(person.Password, model.Password))
                return new AuthenticationResponse(response.AddError(AuthenticationError.EmailOrPasswordIsIncorrect));
            response.Email = person.Email;
            response.Role = person.Role;
            return response;
        }

        /// <summary>
        /// Sign person up if Email and Username are unique
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthenticationResponse SignUp(SignUpModel model)
        {
            var response = new AuthenticationResponse();
            var personByEmail = _personRepository.GetPersonByName(model.Username);
            var personByName = _personRepository.GetPersonByEmail(model.Email);
            if (personByEmail != null)
                return new AuthenticationResponse(response.AddError(AuthenticationError.ThisEmailIsTaken, nameof(model.Email)));
            if (personByName != null)
                return new AuthenticationResponse(response.AddError(AuthenticationError.ThisEmailIsTaken, nameof(model.Username)));
            var person = new Person
            {
                Name = model.Username,
                Password = Crypto.HashPassword(model.Password),
                Email = model.Email
            };
            EncryptService.GetKeys(out var publicKey, out var privateKey);
            person.PublicKey = publicKey;
            person.PrivateKey = privateKey;
            _personRepository.RegisterOrUpdate(person);

            CreateInitialStorage(person.Id);
            response.Email = person.Email;
            response.Role = person.Role;
            return response;
        }

        /// <summary>
        /// Reset person password if Email match
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResponse ResetPassword(PasswordResetModel model)
        {
            var response = new ServiceResponse();
            if (!model.Email.Equals(model.EmailConfirm, StringComparison.OrdinalIgnoreCase))
                return response.AddError(AuthenticationError.EmailsDoNotMatch, nameof(model.Email));
            var person = _personRepository.GetPersonByEmail(model.Email);
            if (person == null) return response.AddError(GeneralError.PersonWasNotFound);
            person.Password = Crypto.HashPassword(model.Password);
            _personRepository.RegisterOrUpdate(person);
            return response;
        }

        /// <summary>
        /// Create storage using first template
        /// </summary>
        /// <param name="personId"></param>
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
