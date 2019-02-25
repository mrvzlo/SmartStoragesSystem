using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;
using SmartKitchen.Domain.Responses;
using System.Linq;
using System.Web.Helpers;

namespace SmartKitchen.DomainService
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
            response.Person = GetPersonByEmail(model.Email);

            if (response.Person == null)
                response.Errors.Add(new ModelStateError(nameof(model.Email), AuthenticationError.UserNotFound));
            else if (!Crypto.VerifyHashedPassword(response.Person.Password, model.Password))
                response.Errors.Add(new ModelStateError(nameof(model.Password), AuthenticationError.EmailOrPasswordIsIncorrect));

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
            _personRepository.Register(new Person
            {
                Name = model.Username,
                Password = Crypto.HashPassword(model.Password),
                Email = model.Email
            });

            response.Person = GetPersonByEmail(model.Email);
            if (response.Person == null)
                response.Errors.Add(new ModelStateError(nameof(model.Email), AuthenticationError.AnErrorOccured));
            else CreateInitialStorage(response.Person.Id);
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
