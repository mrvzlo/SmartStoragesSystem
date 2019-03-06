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
    public class CellService : BaseService, ICellService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IProductService _productService;
        private readonly IStorageRepository _storageRepository;
        private readonly IBasketProductRepository _basketProductRepository;

        public CellService(ICellRepository cellRepository, IProductService productService, IStorageRepository storageRepository, IBasketProductRepository basketProductRepository)
        {
            _cellRepository = cellRepository;
            _productService = productService;
            _storageRepository = storageRepository;
            _basketProductRepository = basketProductRepository;
        }

        public Cell GetOrAddAndGet(CellCreationModel model, string email)
        {
            var productId = _productService.GetOrAddAndGet(model.Product).Id;
            var storage = _storageRepository.GetStorageById(model.Storage);
            if (storage.Person.Email != email) return null;
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
            _cellRepository.AddOrUpdateCell(cell);
            response.AddedId = cell.Id;
            response.AddedGroupId = cell.StorageId;
            return response;
        }

        public CellDisplayModel GetCellDisplayModelById(int id, string email)
        {
            var cell = _cellRepository.GetCellById(id);
            if (cell.Storage.Person.Email != email) return null;
            return GetCellDisplayModel(cell);
        }

        public ServiceResponse UpdateCellAmount(int id, decimal value, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) return null;
            if (value < 0) value = 0;
            cell.Amount = value;
            if (cell.Amount == 0) cell.BestBefore = null;
            _cellRepository.AddOrUpdateCell(cell);
            if (cell.Amount != value) response.AddError(GeneralError.AnErrorOccured);
            return response;
        }

        public ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) return null;
            cell.BestBefore = value;
            _cellRepository.AddOrUpdateCell(cell);
            if (cell.BestBefore != value) response.AddError(GeneralError.AnErrorOccured);
            return response;
        }

        public ServiceResponse DeleteCellById(int id, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) response.AddError(GeneralError.ItemNotFound);
            else
            {
                _basketProductRepository.DeleteBasketProductRange(cell.BasketProducts);
                _cellRepository.DeleteCell(cell);
            }
            return response;
        }

        public IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email)
        {
            var storage = _storageRepository.GetStorageById(storageId);
            if (storage == null || storage.Person.Email != email) return null;
            var query = _cellRepository.GetCellsForStorage(storageId).ProjectTo<CellDisplayModel>(MapperConfig);
            return query;
        }

        private Cell GetCellByProductAndStorage(int product, int storage) =>
            _cellRepository.GetCellByProductAndStorage(product, storage);

        private CellDisplayModel GetCellDisplayModel(Cell cell) => 
            Mapper.Map<CellDisplayModel>(cell);
    }
}
