using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;
using System;
using System.Linq;

namespace SmartKitchen.DomainService.Services
{
    public class BasketService : BaseService, IBasketService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketProductRepository _basketProductRepository;

        public BasketService(IBasketRepository basketRepository, IPersonRepository personRepository, IBasketProductRepository basketProductRepository)
        {
            _basketRepository = basketRepository;
            _personRepository = personRepository;
            _basketProductRepository = basketProductRepository;
        }

        public BasketDisplayModel GetBasketById(int id, string email) => 
            _basketRepository.GetBaskets().Where(x => x.Id == id && x.Person.Email == email).ProjectTo<BasketDisplayModel>(MapperConfig).SingleOrDefault();

        public BasketWithProductsDisplayModel GetBasketWithProductsById(int id, string email) =>
            _basketRepository.GetBaskets().Where(x => x.Id == id && x.Person.Email == email).ProjectTo<BasketWithProductsDisplayModel>(MapperConfig).SingleOrDefault();

        public IQueryable<BasketDisplayModel> GetBasketsByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return person.Baskets.AsQueryable().ProjectTo<BasketDisplayModel>(MapperConfig).OrderByDescending(x => x.CreationDate);
        }

        public ItemCreationResponse AddBasket(NameCreationModel model, string email)
        {
            model.Name = model.Name.Trim();
            var response = new ItemCreationResponse();
            var person = _personRepository.GetPersonByEmail(email);
            var exists = person.Baskets.Any(x => x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
            if (exists) return response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Name));
            var basket = new Basket
            {
                CreationDate = DateTime.Now,
                Name = model.Name,
                PersonId = person.Id
            };
            _basketRepository.AddOrUpdateBasket(basket);
            response.AddedId = basket.Id;
            return response;
        }

        public BasketDisplayModel LockBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketBelongsToPerson(basket, person);
            if (!response.Successful()) return null;

            basket.Closed = true;
            _basketRepository.AddOrUpdateBasket(basket);
            return Mapper.Map<BasketDisplayModel>(basket);
        }

        public bool DeleteBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketBelongsToPerson(basket, person);
            if (!response.Successful()) return false;

            _basketProductRepository.DeleteBasketProductRange(basket.BasketProducts);
            _basketRepository.DeleteBasket(basket);
            return true;
        }
    }
}
