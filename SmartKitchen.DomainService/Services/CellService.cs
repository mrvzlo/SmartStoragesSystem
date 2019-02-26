using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class CellService : BaseService, ICellService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IProductService _productService;
        private readonly IPersonService _personService;
        private readonly IStorageRepository _storageRepository;

        public CellService(ICellRepository cellRepository, IProductService productService, IPersonService personService, IStorageRepository storageRepository)
        {
            _cellRepository = cellRepository;
            _productService = productService;
            _personService = personService;
            _storageRepository = storageRepository;
        }

        public Cell GetOrAddAndGet(CellCreationModel model, string email)
        {
            var productId = _productService.GetOrAddAndGet(model.Product).Id;
            var cell = GetCellByProductAndStorage(productId, model.Storage);
            if (cell != null) return cell;
            var creation = AddCell(model, email);
            return !creation.Successful() ? null : _cellRepository.GetCellById(creation.AddedId);
        }

        public ItemCreationResponse AddCell(CellCreationModel model, string email)
        {
            var response = new ItemCreationResponse();
            var storage = _storageRepository.GetStorageById(model.Storage);
            var storageAccessError = _personService.StorageAccessError(storage, email);
            if (storageAccessError != null)
            {
                response.Errors.Add(storageAccessError);
                return response;
            }

            var productId = _productService.GetOrAddAndGet(model.Product).Id;
            if (GetCellByProductAndStorage(productId,model.Storage) != null) response.AddError(GeneralError.NameIsAlreadyTaken,"Name");
            else
            {
                var cell = new Cell
                {
                    ProductId = productId,
                    Storage = model.Storage,
                    BestBefore = null
                };
                _cellRepository.AddCell(cell);
                if (cell.Id > 0)
                {
                    response.AddedId = cell.Id;
                    response.AddedGroupId = cell.Storage;
                }
                else response.AddError(GeneralError.AnErrorOccured);
            }
            return response;
        }

        public Cell GetCellByProductAndStorage(int product, int storage) => 
            _cellRepository.GetCellByProductAndStorage(product, storage);
        
    }
}
