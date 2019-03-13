using System.Collections.Generic;
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
            var response = new ItemCreationResponse();
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
            var response = new ItemCreationResponse();
            if (basket == null || storage == null)
            {
                response.AddError(GeneralError.ItemNotFound);
                return response;
            }

            if (basket.PersonId != person.Id || storage.PersonId != person.Id)
            {
                response.AddError(GeneralError.AccessDenied);
                return response;
            }

            var basketProduct = new BasketProduct
            {
                BasketId = basket.Id,
                CellId = cellId,
                BestBefore = null
            };
            _basketProductRepository.AddBasketProduct(basketProduct);
            response.AddedGroupId = basketProduct.BasketId;
            response.AddedId = basketProduct.Id;
            return response;
        }

        public BasketProductDisplayModel GetBasketProductDisplayModelById(int id, string email)
        {
            var basketProduct = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(basketProduct.BasketId);
            int personId = _personRepository.GetPersonByEmail(email).Id;
            if (basket == null || basket.PersonId != personId) return null;
            return Mapper.Map<BasketProductDisplayModel>(basketProduct);
        }
    }
}
