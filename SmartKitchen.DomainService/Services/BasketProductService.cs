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

        /// <summary>
        /// Get cell by model properties and assign it to new basket product
        /// </summary>
        /// <param name="model"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ItemCreationResponse AddBasketProductByModel(BasketProductCreationModel model, string email)
        {
            var basket = _basketRepository.GetBasketById(model.Basket);
            var storage = _storageRepository.GetStorageById(model.Storage);
            var person = _personRepository.GetPersonByEmail(email);
            var cellId = _cellService.GetOrAddAndGetCell(Mapper.Map<CellCreationModel>(model), email).Id;
            return AddBasketProduct(storage, basket, person, cellId);
        }

        /// <summary>
        /// Add multiple products for one storage
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="storageId"></param>
        /// <param name="email"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int AddBasketProductList(int basketId, int storageId, string email, List<int> cells)
        {
            var count = 0;
            var basket = _basketRepository.GetBasketById(basketId);
            var storage = _storageRepository.GetStorageById(storageId);
            var person = _personRepository.GetPersonByEmail(email);
            if (cells == null) return count;
            foreach (var c in cells)
            {
                var response = AddBasketProduct(storage, basket, person, c);
                if (response.Successful()) count++;
            }

            return count;
        }

        /// <summary>
        /// Add basket product to cell in storage
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="basket"></param>
        /// <param name="person"></param>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public ItemCreationResponse AddBasketProduct(Storage storage, Basket basket, Person person, int cellId)
        {
            var response = new ItemCreationResponse(BasketBelongsToPerson(basket, person));
            if (!response.Successful()) return response;
            response = new ItemCreationResponse(StorageBelongsToPerson(storage, person));
            if (!response.Successful()) return response;

            var basketProduct = _basketProductRepository.GetBasketProductByBasketAndCell(basket.Id, cellId);
            if (basketProduct != null)
                return response.AddError(GeneralError.NameIsAlreadyTaken);

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

        /// <summary>
        /// Get list of products in basket
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public IQueryable<BasketProductDisplayModel> GetBasketProductDisplayModelByBasket(int basketId, string email)
        {
            var basket = _basketRepository.GetBasketById(basketId);
            var person = _personRepository.GetPersonByEmail(email);
            return BasketBelongsToPerson(basket, person).Successful() 
                ? basket.BasketProducts.AsQueryable().ProjectTo<BasketProductDisplayModel>(MapperConfig) 
                : null;
        }

        /// <summary>
        /// Mark product in open basket 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse MarkProductBought(int id, bool status, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.Bought = status;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        /// <summary>
        /// Update basket product amount and status if out of product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse UpdateProductAmount(int id, int value, string email)
        {
            if (value < 0) return new ServiceResponse().AddError(GeneralError.NegativeNumber);
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            if (value < 0) value = 0;
            if (value == 0) product.BestBefore = null;
            product.Amount = value;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        /// <summary>
        /// Update best before with any date - past or future
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse UpdateProductBestBefore(int id, DateTime? value, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.BestBefore = value;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        /// <summary>
        /// Update product price not according to amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse UpdateProductPrice(int id, decimal value, string email)
        {
            if (value < 0) return new ServiceResponse().AddError(GeneralError.NegativeNumber);
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            product.Price = value;
            _basketProductRepository.AddOrUpdateBasketProduct(product);
            return response;
        }

        /// <summary>
        /// Delete product if it belongs to request sender
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse DeleteBasketProductByIdAndEmail(int id, string email)
        {
            var product = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(product.BasketId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = BasketProductBelongsToPerson(product, person, basket);
            if (!response.Successful()) return response;

            _basketProductRepository.DeleteBasketProduct(product);
            return response;
        }
    }
}
