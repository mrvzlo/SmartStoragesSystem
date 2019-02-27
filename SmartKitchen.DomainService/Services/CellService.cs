using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
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
            if (storage == null) response.AddError(GeneralError.ItemNotFound, nameof(model.Storage));
            else if (storage.Person.Email != email) response.AddError(GeneralError.AccessDenied, nameof(model.Storage));
            if (!response.Successful()) return response;

            var productId = _productService.GetOrAddAndGet(model.Product).Id;
            if (GetCellByProductAndStorage(productId, model.Storage) != null)
            {
                response.AddError(GeneralError.NameIsAlreadyTaken, nameof(model.Product));
                return response;
            }

            var cell = new Cell
            {
                ProductId = productId,
                StorageId = model.Storage,
                BestBefore = null
            };
            _cellRepository.AddCell(cell);
            response.AddedId = cell.Id;
            response.AddedGroupId = cell.StorageId;
            return response;
        }

        public Cell GetCellByProductAndStorage(int product, int storage) =>
            _cellRepository.GetCellByProductAndStorage(product, storage);

        public CellDisplayModel GetCellDisplayModelById(int id, string email)
        {
            var cell = _cellRepository.GetCellById(id);
            if (cell.Storage.Person.Email != email) return null;
            var result = Mapper.Map<CellDisplayModel>(cell);
            return result;
        }
    }
}
