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
            if (basketProduct == null || person == null) return new ServiceResponse().AddError(GeneralError.ItemNotFound);
            return BasketBelongsToPerson(basket, person);
        }

        public ServiceResponse BasketBelongsToPerson(Basket basket, Person person)
        {
            if (basket == null || person == null) return new ServiceResponse().AddError(GeneralError.ItemNotFound);
            if (basket.PersonId != person.Id) return new ServiceResponse().AddError(GeneralError.AccessDenied);
            return new ServiceResponse();
        }

        public ServiceResponse CellBelongsToPerson(Cell cell, Person person, Storage storage)
        {
            if (cell == null || person == null) return new ServiceResponse().AddError(GeneralError.ItemNotFound);
            return StorageBelongsToPerson(storage, person);
        }

        public ServiceResponse StorageBelongsToPerson(Storage storage, Person person)
        {
            if (storage == null || person == null) return new ServiceResponse().AddError(GeneralError.ItemNotFound);
            if (storage.PersonId != person.Id) return new ServiceResponse().AddError(GeneralError.AccessDenied);
            return new ServiceResponse();
        }
    }
}
