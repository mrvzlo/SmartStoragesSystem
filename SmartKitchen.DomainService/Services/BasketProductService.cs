using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class BasketProductService : BaseService, IBasketProductService
    {
        private readonly ICellService _cellService;
        private readonly IStorageRepository _storageRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IPersonRepository _personRepository;

        public BasketProductService(IBasketRepository basketRepository, IBasketProductRepository basketProductRepository,
            IStorageRepository storageRepository, ICellService cellService, IPersonRepository personRepository)
        {
            _cellService = cellService;
            _basketRepository = basketRepository;
            _storageRepository = storageRepository;
            _basketProductRepository = basketProductRepository;
            _personRepository = personRepository;
        }

        public ItemCreationResponse AddBasketProductByModel(BasketProductCreationModel model, string email)
        {
            var basket = _basketRepository.GetBasketById(model.Basket);
            var storage = _storageRepository.GetStorageById(model.Storage);
            var person = _personRepository.GetPersonByEmail(email);
            var cellId = _cellService.GetOrAddAndGet(Mapper.Map<CellCreationModel>(model), email).Id;
            return AddBasketProduct(storage, basket, person, cellId);
        }

        public int AddBasketProductList(int basketId, int storageId, string email, List<int> cells)
        {
            int count = 0;
            var basket = _basketRepository.GetBasketById(basketId);
            var storage = _storageRepository.GetStorageById(storageId);
            var person = _personRepository.GetPersonByEmail(email);
            foreach (var c in cells)
            {
                var response = AddBasketProduct(storage, basket, person, c);
                if (response.Successful()) count++;
            }

            return count;
        }

        public ItemCreationResponse AddBasketProduct(Storage storage, Basket basket, Person person, int cellId)
        {
            var response = (ItemCreationResponse)BasketBelongsToPerson(basket, person);
            if (!response.Successful()) return response;
            response = (ItemCreationResponse)StorageBelongsToPerson(storage, person);
            if (!response.Successful()) return response;

            var basketProduct = _basketProductRepository.GetBasketProductByBasketAndCell(basket.Id, cellId);
            if (basketProduct != null)
                return (ItemCreationResponse)response.AddError(GeneralError.NameIsAlreadyTaken);

            basketProduct = new BasketProduct
            {
                BasketId = basket.Id,
                CellId = cellId,
                BestBefore = null
            };
            _basketProductRepository.AddOrUpdateBasketProduct(basketProduct);
            response.AddedGroupId = basketProduct.BasketId;
            response.AddedId = basketProduct.Id;
            return response;
        }

        public IQueryable<BasketProductDisplayModel> GetBasketProductDisplayModelByBasket(int basketId, string email)
        {
            var basket = _basketRepository.GetBasketById(basketId);
            var person = _personRepository.GetPersonByEmail(email);
            return BasketBelongsToPerson(basket, person).Successful() 
                ? basket.BasketProducts.AsQueryable().ProjectTo<BasketProductDisplayModel>(MapperConfig) 
                : null;
        }

        public ServiceResponse MarkProductBought(int id, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = ProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.Bought = !product.Bought;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        public ServiceResponse UpdateProductAmount(int id, int value, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = ProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            if (value < 0) value = 0;
            if (value == 0)
            {
                product.BestBefore = null;
                _basketProductRepository.AddOrUpdateBasketProduct(product);
            }
            return response;
        }

        public ServiceResponse UpdateProductBestBefore(int id, DateTime? value, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = ProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.BestBefore = value;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        public ServiceResponse UpdateProductPrice(int id, int value, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = ProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.Amount = value;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        public ServiceResponse DeleteBasketProductByIdAndEmail(int id, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = ProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            _basketProductRepository.DeleteBasketProduct(product);
            return response;
        }
    }
}
