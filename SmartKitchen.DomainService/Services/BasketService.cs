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
        private readonly ICellService _cellService;

        public BasketService(IBasketRepository basketRepository, IPersonRepository personRepository, IBasketProductRepository basketProductRepository, ICellService cellService)
        {
            _cellService = cellService;
            _basketRepository = basketRepository;
            _personRepository = personRepository;
            _basketProductRepository = basketProductRepository;
        }

        public BasketDisplayModel GetBasketById(int id, string email) => 
            _basketRepository.GetBaskets().Where(x => x.Id == id && x.Person.Email == email).ProjectTo<BasketDisplayModel>(MapperConfig).SingleOrDefault();
        
        public IQueryable<BasketDisplayModel> GetBasketsByOwnerEmail(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return person.Baskets.AsQueryable().ProjectTo<BasketDisplayModel>(MapperConfig).OrderByDescending(x => x.CreationDate);
        }

        /// <summary>
        /// Add basket if name is unique
        /// </summary>
        /// <param name="model"></param>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update basket name if new name is unique and basket belongs to request sender
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool UpdateBasketName(NameCreationModel model, int id, string email)
        {
            model.Name = model.Name.Trim();
            var basket = _basketRepository.GetBasketById(id);
            var person = _personRepository.GetPersonByEmail(email);
            if (!BasketBelongsToPerson(basket, person).Successful()) return false;
            if (_basketRepository.GetBaskets().Any(x => x.PersonId == person.Id && x.Name.Equals(model.Name))) return false;
            basket.Name = model.Name;
            _basketRepository.AddOrUpdateBasket(basket);
            return true;
        }

        /// <summary>
        /// Delete basket 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// If basket is open execute cell update for all basket products 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public int FinishAndCloseBasket(int id, string email)
        {
            var count = 0;
            var basket = _basketRepository.GetBasketById(id);
            if (basket.Closed) return count;
            var person = _personRepository.GetPersonByEmail(email);
            if (!BasketBelongsToPerson(basket, person).Successful()) return count;
            var products = basket.BasketProducts;
            count = products.Select(product => _cellService.MoveBasketProductToStorage(product, basket, person)).Count(response => response.Successful());

            basket.Closed = true;
            _basketRepository.AddOrUpdateBasket(basket);
            return count;
        }

        public bool ReopenBasket(int id, string email)
        {
            var basket = _basketRepository.GetBasketById(id);
            var person = _personRepository.GetPersonByEmail(email);
            if (!BasketBelongsToPerson(basket, person).Successful()) return false;
            if (!basket.Closed) return true;
            basket.Closed = false;
            _basketRepository.AddOrUpdateBasket(basket);
            _basketProductRepository.UnmarkBasketProducts(id);
            return !basket.Closed;
        }
    }
}
