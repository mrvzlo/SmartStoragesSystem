﻿using AutoMapper.QueryableExtensions;
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

        public ItemCreationResponse AddCell(CellCreationModel model, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return AddCell(model, person);
        }

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

        public ServiceResponse UpdateCellAmount(int id, int value, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return UpdateCellAmount(id, value, person);
        }

        public ServiceResponse UpdateCellAmount(int id, int value, Person person)
        {
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

        public ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return UpdateCellBestBefore(id, value, person);
        }

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

        public ServiceResponse DeleteCellById(int id, string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return DeleteCellById(id, person);
        }

        public ServiceResponse DeleteCellById(int id, Person person)
        {
            var cell = _cellRepository.GetCellById(id);
            var storage = _storageRepository.GetStorageById(cell.StorageId);
            var response = CellBelongsToPerson(cell, person, storage);
            if (!response.Successful()) return response;
            DeleteCell(cell);
            return response;
        }

        public void DeleteCell(Cell cell)
        {
            _basketProductRepository.DeleteBasketProductRange(cell.BasketProducts);
            _cellRepository.DeleteCellAmountChanges(cell.CellChanges);
            _cellRepository.DeleteCell(cell);
        }

        public IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email)
        {
            var storage = _storageRepository.GetStorageById(storageId);
            var person = _personRepository.GetPersonByEmail(email);
            var response = StorageBelongsToPerson(storage, person);
            return !response.Successful() ? null : _cellRepository.GetCellsForStorage(storageId).ProjectTo<CellDisplayModel>(MapperConfig);
        }
        
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
