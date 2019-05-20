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

        /// <summary>
        /// Generate new key pair if request sender exists
        /// </summary>
        /// <param name="email"></param>
        public void UpdateKeyPair(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            if (person == null) return;
            EncryptService.GetKeys(out var publicKey, out var privateKey);
            person.PublicKey = publicKey;
            person.PrivateKey = privateKey;
            _personRepository.RegisterOrUpdate(person);
        }

        /// <summary>
        /// Split request, get private key using person id, decode text, split parameters and switch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResponse Interpretator(string request)
        {
            var response = new ServiceResponse();
            var requestSplit = request.Split(':');
            var person = _personRepository.GetPersonById(Convert.ToInt32(requestSplit[0]));
            if (person == null) return response.AddError(GeneralError.PersonWasNotFound);
            var description = EncryptService.Decrypt(requestSplit[1], person.PrivateKey);
            var tokenRequest = new CryptRequest(description.Split(':'));
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
