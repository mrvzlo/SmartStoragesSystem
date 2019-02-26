using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class BasketService : BaseService, IBasketService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository, IPersonRepository personRepository)
        {
            _basketRepository = basketRepository;
            _personRepository = personRepository;
        }

        public List<BasketDisplayModel> GetBasketsWithDescriptionByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return _basketRepository.GetAllUserBaskets(person.Id).ProjectTo<BasketDisplayModel>(MapperConfig).ToList();
        }

        public ItemCreationResponse CreateBasket(NameCreationModel name, string email)
        {
            var response = new ItemCreationResponse();
            var personId = _personRepository.GetPersonByEmail(email).Id;
            var exists = _basketRepository.GetBasketByNameAndOwner(name.Value, personId) != null;
            if (exists)
            {
                response.Errors.Add(new ModelStateError("Name", null)); //todo
                return response;
            }
            var basket = new Basket
            {
                CreationDate = DateTime.Now,
                Name = name.Value.Trim(),
                Owner = personId
            };
            _basketRepository.AddBasket(basket);
            if (basket.Id <= 0) return response;
            response.Id = basket.Id;
            response.Success();
            return response;
        }
    }
}
