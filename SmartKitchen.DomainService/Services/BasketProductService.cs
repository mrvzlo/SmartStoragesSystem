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
        private readonly IPersonService _personService;
        private readonly IStorageRepository _storageRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketProductRepository _basketProductRepository;

        public BasketProductService(IBasketRepository basketRepository, IBasketProductRepository basketProductRepository,
            IStorageRepository storageRepository, ICellService cellService, IPersonService personService)
        {
            _cellService = cellService;
            _personService = personService;
            _basketRepository = basketRepository;
            _storageRepository = storageRepository;
            _basketProductRepository = basketProductRepository;
        }

        public ItemCreationResponse AddBasketProduct(BasketProductCreationModel model, string email)
        {
            var response = new ItemCreationResponse();
            var basket = _basketRepository.GetBasketById(model.Basket);
            var storage = _storageRepository.GetStorageById(model.Storage);
            if (basket == null || storage == null)
            {
                response.AddError(GeneralError.ItemNotFound);
                return response;
            }

            if (basket.Person.Email != email || storage.Person.Email != email)
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
            if (basket == null || basket.Person.Email != email) return null;
            return Mapper.Map<BasketProductDisplayModel>(basket);
        }
    }
}
