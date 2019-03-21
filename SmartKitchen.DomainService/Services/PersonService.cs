using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICellService _cellService;

        public PersonService(IPersonRepository personRepository, ICellService cellService)
        {
            _personRepository = personRepository;
            _cellService = cellService;
        }

        public Person GetPersonByEmail(string email) =>
            _personRepository.GetPersonByEmail(email);

        public void UpdateKeyPair(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            if (person == null) return;
            EncryptService.GetKeys(out var publicKey, out var privateKey);
            person.PublicKey = publicKey;
            person.PrivateKey = privateKey;
            _personRepository.RegisterOrUpdate(person);
        }

        public ServiceResponse Interpretator(string request)
        {
            var response = new ServiceResponse();
            var requestSplit = request.Split(':');
            var person = _personRepository.GetPersonById(Convert.ToInt32(requestSplit[0]));
            if (person == null) return response.AddError(GeneralError.PersonWasNotFound);
            var decription = EncryptService.Decrypt(requestSplit[1], person.PrivateKey);
            var tokenRequest = new CryptRequest(decription.Split(':'));
            switch (tokenRequest.Action)
            {
                case "AddCell":
                    return _cellService.AddCell( new CellCreationModel { Product = tokenRequest.Value, Storage = tokenRequest.Object }, person);
                case "RemoveCell":
                    return _cellService.DeleteCellById(tokenRequest.Object, person);
                case "UpdateCellBestBefore":
                    return _cellService.UpdateCellBestBefore(tokenRequest.Object, DateTime.ParseExact(tokenRequest.Value,"yyyy-MM-dd",null), person);
                case "UpdateCellAmount":
                    return _cellService.UpdateCellAmount(tokenRequest.Object,Convert.ToInt32(tokenRequest.Value), person);
                default:
                    return response.AddError(GeneralError.UnknownCommand);
            }
        }
    }
}
