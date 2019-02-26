using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.Enums;

namespace SmartKitchen.DomainService.Services
{
    public class BasketService : BaseService, IBasketService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IPersonService _personService;

        public BasketService(IBasketRepository basketRepository, IPersonRepository personRepository, IPersonService personService)
        {
            _basketRepository = basketRepository;
            _personRepository = personRepository;
            _personService = personService;
        }

        public BasketDisplayModel GetBasketById(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            return _personService.BasketAccessError(basket, email) != null ? null : Mapper.Map<BasketDisplayModel>(basket);
        }

        public BasketWithProductsDisplayModel GetBasketWithProductsById(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            return _personService.BasketAccessError(basket, email) != null ? null : Mapper.Map<BasketWithProductsDisplayModel>(basket);
        }

        public List<BasketDisplayModel> GetBasketsByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return _basketRepository.GetAllUserBaskets(person.Id).ProjectTo<BasketDisplayModel>(MapperConfig).ToList();
        }

        public ItemCreationResponse AddBasket(NameCreationModel name, string email)
        {
            var response = new ItemCreationResponse();
            var personId = _personRepository.GetPersonByEmail(email).Id;
            var exists = _basketRepository.GetBasketByNameAndOwner(name.Value, personId) != null;
            if (exists)
            {
                response.AddError(GeneralError.NameIsAlreadyTaken,"Name");
                return response;
            }
            var basket = new Basket
            {
                CreationDate = DateTime.Now,
                Name = name.Value.Trim(),
                Owner = personId
            };
            _basketRepository.AddBasket(basket);
            if (basket.Id > 0) response.Id = basket.Id;
            else response.AddError(GeneralError.AnErrorOccured);
            return response;
        }

        public bool LockBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            if (_personService.BasketAccessError(basket, email) != null) return false;
            _basketRepository.LockBasketById(id);
            return true;
        }

        public bool DeleteBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            if (_personService.BasketAccessError(basket, email) != null) return false;
            _basketRepository.DeleteBasket(basket);
            return true;
        }
    }
}
