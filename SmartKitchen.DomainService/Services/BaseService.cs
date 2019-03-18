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

        public ServiceResponse ProductBelongsToPerson(BasketProduct basketProduct, Person person, Basket basket)
        {
            if (person == null) return new ServiceResponse().AddError(GeneralError.PersonWasNotFound);
            if (basketProduct == null) return new ServiceResponse().AddError(GeneralError.BasketProductWasNotFound);
            return BasketBelongsToPerson(basket, person);
        }

        public ServiceResponse BasketBelongsToPerson(Basket basket, Person person)
        {
            if (person == null) return new ServiceResponse().AddError(GeneralError.PersonWasNotFound);
            if (basket == null) return new ServiceResponse().AddError(GeneralError.BasketWasNotFound);
            if (basket.PersonId != person.Id) return new ServiceResponse().AddError(GeneralError.AccessToBasketDenied);
            return new ServiceResponse();
        }

        public ServiceResponse CellBelongsToPerson(Cell cell, Person person, Storage storage)
        {
            if (person == null) return new ServiceResponse().AddError(GeneralError.PersonWasNotFound);
            if (cell == null) return new ServiceResponse().AddError(GeneralError.CellWasNotFound);
            return StorageBelongsToPerson(storage, person);
        }

        public ServiceResponse StorageBelongsToPerson(Storage storage, Person person)
        {
            if (person == null) return new ServiceResponse().AddError(GeneralError.PersonWasNotFound);
            if (storage == null) return new ServiceResponse().AddError(GeneralError.StorageWasNotFound);
            if (storage.PersonId != person.Id) return new ServiceResponse().AddError(GeneralError.AccessToStorageDenied);
            return new ServiceResponse();
        }
    }
}
