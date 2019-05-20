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
        private readonly IPersonRepository _personRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IBasketProductRepository _basketProductRepository;

        public CellService(ICellRepository cellRepository, IProductService productService, IStorageRepository storageRepository,
            IBasketProductRepository basketProductRepository, IPersonRepository personRepository)
        {
            _cellRepository = cellRepository;
            _productService = productService;
            _storageRepository = storageRepository;
            _basketProductRepository = basketProductRepository;
            _personRepository = personRepository;
        }

        /// <summary>
        /// If exists get else create and check creation successfulness
        /// </summary>
        /// <param name="model"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Cell GetOrAddAndGetCell(CellCreationModel model, string email)
        {
            var storage = _storageRepository.GetStorageById(model.Storage);
            var person = _personRepository.GetPersonByEmail(email);
            if (!StorageBelongsToPerson(storage, person).Successful()) return null;

            var productId = _productService.GetOrAddAndGetProduct(model.Product).Id;
            var cell = _cellRepository.GetCellByProductAndStorage(productId, model.Storage);
            if (cell != null) return cell;
            var creation = AddCell(model, email);
            return !creation.Successful() ? null : _cellRepository.GetCellById(creation.AddedId);
        }

        /// <summary>
        /// Override cell creation for unspecified request sender
        /// </summary>
        /// <param name="model"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ItemCreationResponse AddCell(CellCreationModel model, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return AddCell(model, person);
        }

        /// <summary>
        /// Create new empty cell if it does not exist
        /// </summary>
        /// <param name="model"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public ItemCreationResponse AddCell(CellCreationModel model, Person person)
        {
            var response = new ItemCreationResponse();
            var storage = _storageRepository.GetStorageById(model.Storage);
            var check = StorageBelongsToPerson(storage, person);
            if (!check.Successful()) return new ItemCreationResponse(check);

            var productId = _productService.GetOrAddAndGetProduct(model.Product).Id;
            if (_cellRepository.GetCellByProductAndStorage(productId, model.Storage) != null)
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
            _cellRepository.AddCellAmountChange(new CellChange { Amount = 0, CellId = cell.Id, UpdateDate = DateTime.UtcNow });
            response.AddedId = cell.Id;
            response.AddedGroupId = cell.StorageId;
            return response;
        }

        /// <summary>
        /// Override cell amount update for unspecified request sender
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse UpdateCellAmount(int id, int value, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return UpdateCellAmount(id, value, person);
        }

        /// <summary>
        /// Create new cell amount record and change status if out of product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public ServiceResponse UpdateCellAmount(int id, int value, Person person)
        {
            if (value<0) return new ServiceResponse().AddError(GeneralError.NegativeNumber);
            var cell = _cellRepository.GetCellById(id);
            var storage = _storageRepository.GetStorageById(cell.StorageId);
            var response = CellBelongsToPerson(cell, person, storage);
            if (!response.Successful()) return response;

            if (value < 0) value = 0;
            if (value == 0)
            {
                cell.BestBefore = null;
                _cellRepository.AddOrUpdateCell(cell);
            }

            _cellRepository.AddCellAmountChange(new CellChange { Amount = value, CellId = cell.Id, UpdateDate = DateTime.UtcNow });
            return response;
        }

        /// <summary>
        /// Override cell best before update for unspecified request sender
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return UpdateCellBestBefore(id, value, person);
        }

        /// <summary>
        /// Replace best before with any date - past or future
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public ServiceResponse UpdateCellBestBefore(int id, DateTime? value, Person person)
        {
            var cell = _cellRepository.GetCellById(id);
            var storage = _storageRepository.GetStorageById(cell.StorageId);
            var response = CellBelongsToPerson(cell, person, storage);
            if (!response.Successful()) return response;

            cell.BestBefore = value;
            _cellRepository.AddOrUpdateCell(cell);
            return response;
        }

        /// <summary>
        /// Override cell removing for unspecified request sender
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResponse DeleteCellById(int id, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return DeleteCellById(id, person);
        }

        /// <summary>
        /// Remove cell if it belongs to request sender
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public ServiceResponse DeleteCellById(int id, Person person)
        {
            var cell = _cellRepository.GetCellById(id);
            var storage = _storageRepository.GetStorageById(cell.StorageId);
            var response = CellBelongsToPerson(cell, person, storage);
            if (!response.Successful()) return response;
            DeleteCell(cell);
            return response;
        }

        /// <summary>
        /// Delete cell with all tied records
        /// </summary>
        /// <param name="cell"></param>
        public void DeleteCell(Cell cell)
        {
            _basketProductRepository.DeleteBasketProductRange(cell.BasketProducts);
            _cellRepository.DeleteCellAmountChanges(cell.CellChanges);
            _cellRepository.DeleteCell(cell);
        }

        /// <summary>
        /// Get cells if their storage belongs to request sender
        /// </summary>
        /// <param name="storageId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email)
        {
            var storage = _storageRepository.GetStorageById(storageId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = StorageBelongsToPerson(storage, person);
            return !response.Successful() ? null : _cellRepository.GetCellsForStorage(storageId).ProjectTo<CellDisplayModel>(MapperConfig);
        }
        
        /// <summary>
        /// Update cell properties if basket product is bought and both basket and cell belong to one request sender
        /// </summary>
        /// <param name="basketProduct"></param>
        /// <param name="basket"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public ItemCreationResponse MoveBasketProductToStorage(BasketProduct basketProduct, Basket basket, Person person)
        {
            var response = new ItemCreationResponse();
            if (!basketProduct.Bought) return response.AddError(GeneralError.ProductIsNotBought);
            var cell = _cellRepository.GetCellById(basketProduct.CellId);
            var storage = _storageRepository.GetStorageById(cell.StorageId);
            response = new ItemCreationResponse(CellBelongsToPerson(cell, person, storage));
            if (!response.Successful()) return response;
            var lastChange = cell.CellChanges.OrderByDescending(x => x.UpdateDate).First().Amount;
            cell.BestBefore = basketProduct.BestBefore;
            var cellChange = new CellChange
            {
                CellId = basketProduct.CellId,
                Amount = lastChange + basketProduct.Amount,
                UpdateDate = DateTime.Now,
            };
            _cellRepository.AddOrUpdateCell(cell);
            _cellRepository.AddCellAmountChange(cellChange);
            return response;
        }
    }
}
