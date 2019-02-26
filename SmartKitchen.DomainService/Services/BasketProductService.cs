using System.Web.UI.WebControls;
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
            var basketAccessError = _personService.BasketAccessError(basket, email);
            var storageAccessError = _personService.StorageAccessError(storage, email);
            if (basketAccessError != null)
            {
                response.Errors.Add(basketAccessError);
                return response;
            }
            if (storageAccessError != null)
            {
                response.Errors.Add(storageAccessError);
                return response;
            }

            var cellId = _cellService.GetOrCreateAndGet(Mapper.Map<CellCreationModel>(model)).Id;
            var basketProduct = new BasketProduct
            {
                BasketId = model.Basket,
                CellId = cellId,
                BestBefore = null
            };
            _basketProductRepository.AddBasketProduct(basketProduct);
            if (basketProduct.Id > 0) response.Id = basketProduct.Id;
            else response.AddError(GeneralError.AnErrorOccured);
            return response;
        }

        public BasketProductDisplayModel GetBasketProductDisplayModelById(int id, string email)
        {
            var basketProduct = _basketProductRepository.GetBasketProductById(id);
            var basket = _basketRepository.GetBasketById(basketProduct.BasketId);
            if (_personService.BasketAccessError(basket, email) != null) return null;
            return Mapper.Map<BasketProductDisplayModel>(basket);
        }
    }
}
