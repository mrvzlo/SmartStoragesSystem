using System;
using AutoMapper;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class BaseService
    {
        /// <summary>
        /// Auto-injected lazy instance of AutoMapper
        /// </summary>
        public Lazy<IMapper> MapperInstance { protected get; set; }
        
        /// <summary>
        /// The AutoMapper
        /// </summary>
        protected IMapper Mapper => MapperInstance.Value;

        /// <summary>
        /// The AutoMapper Configuration
        /// </summary>
        protected IConfigurationProvider MapperConfig => Mapper.ConfigurationProvider;

        protected static string TitledString(string src) =>
            src.Length < 1 ? "" : src[0].ToString().ToUpper() + (src.Length < 2 ? "" : src.Substring(1).ToLower());

        protected ServiceResponse BasketBelongsToPerson(Basket basket, Person person)
        {
            var response = new ServiceResponse();
            if (person == null) return response.AddError(GeneralError.PersonWasNotFound);
            if (basket == null) return response.AddError(GeneralError.BasketWasNotFound);
            return basket.PersonId != person.Id ? response.AddError(GeneralError.AccessToBasketDenied) : response;
        }
        protected ServiceResponse StorageBelongsToPerson(Storage storage, Person person)
        {
            var response = new ServiceResponse();
            if (person == null) return response.AddError(GeneralError.PersonWasNotFound);
            if (storage == null) return response.AddError(GeneralError.StorageWasNotFound);
            return storage.PersonId != person.Id ? response.AddError(GeneralError.AccessToStorageDenied) : response;
        }

        protected ServiceResponse BasketProductBelongsToPerson(BasketProduct basketProduct, Person person, Basket basket) => 
            basketProduct == null ? new ServiceResponse().AddError(GeneralError.BasketProductWasNotFound) : BasketBelongsToPerson(basket, person);

        protected ServiceResponse CellBelongsToPerson(Cell cell, Person person, Storage storage) => 
            cell == null ? new ServiceResponse().AddError(GeneralError.CellWasNotFound) : StorageBelongsToPerson(storage, person);

    }
}
