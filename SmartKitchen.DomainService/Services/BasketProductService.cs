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

        public ItemCreationResponse AddBasketProduct(BasketProductCreationModel model, string email)
        {
            var response = new ItemCreationResponse();
            var basket = _basketRepository.GetBasketById(model.Basket);
            var storage = _storageRepository.GetStorageById(model.Storage);
            int personId = _personRepository.GetPersonByEmail(email).Id;
            if (basket == null || storage == null)
            {
                response.AddError(GeneralError.ItemNotFound);
                return response;
            }

            if (basket.PersonId != personId || storage.PersonId != personId)
            {
                response.AddError(GeneralError.AccessDenied);
                return response;
            }

            var cellId = _cellService.GetOrAddAndGet(Mapper.Map<CellCreationModel>(model), email).Id;
            var basketProduct = new BasketProduct
            {
                BasketId = model.Basket,
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
