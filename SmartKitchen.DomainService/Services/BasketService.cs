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
            return basket == null || basket.Person.Email != email ? null : Mapper.Map<BasketDisplayModel>(basket);
        }

        public BasketWithProductsDisplayModel GetBasketWithProductsById(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            return basket == null || basket.Person.Email != email ? null : Mapper.Map<BasketWithProductsDisplayModel>(basket);
        }
        
        public IQueryable<BasketDisplayModel> GetBasketsByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return Mapper.Map<IQueryable<BasketDisplayModel>>(person.Baskets);
        }

        public ItemCreationResponse AddBasket(NameCreationModel model, string email)
        {
            model.Name = model.Name.Trim();
            var response = new ItemCreationResponse();
            var personId = _personRepository.GetPersonByEmail(email).Id;
            var exists = _basketRepository.GetBasketByNameAndOwner(model.Name, personId) != null;
            if (exists)
            {
                response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
                return response;
            }
            var basket = new Basket
            {
                CreationDate = DateTime.Now,
                Name = model.Name,
                Owner = personId
            };
            _basketRepository.AddBasket(basket);
            response.AddedId = basket.Id;
            return response;
        }

        public bool LockBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            if (basket == null || basket.Person.Email != email) return false;
            _basketRepository.LockBasketById(id);
            return true;
        }

        public bool DeleteBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            if (basket == null || basket.Person.Email != email) return false;
            _basketRepository.DeleteBasket(basket);
            return true;
        }
    }
}
