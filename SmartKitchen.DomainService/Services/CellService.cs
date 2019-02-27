﻿using System;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain;
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
        private readonly IStorageRepository _storageRepository;

        public CellService(ICellRepository cellRepository, IProductService productService, IStorageRepository storageRepository)
        {
            _cellRepository = cellRepository;
            _productService = productService;
            _storageRepository = storageRepository;
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
            result.SafetyStatus = GetSafetyStatusByDatetime(result.BestBefore);
            return result;
        }

        public ServiceResponse UpdateCellAmount(int id, int value, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) response.AddError(GeneralError.ItemNotFound);
            else
            {
                if (value > (int)Amount.Plenty) value = (int)Amount.Plenty;
                else if (value < (int)Amount.None) value = (int)Amount.None;
                _cellRepository.UpdateAmount(id, value);
            }
            return response;
        }

        public ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) response.AddError(GeneralError.ItemNotFound);
            else
            {
                _cellRepository.UpdateDatetime(id, value);
            }
            return response;
        }

        public ServiceResponse DeleteCellById(int id, string email)
        {
            var response = new ServiceResponse();
            var cell = _cellRepository.GetCellById(id);
            if (cell == null || cell.Storage.Person.Email != email) response.AddError(GeneralError.ItemNotFound);
            else _cellRepository.DeleteCell(cell);
            return response;
        }

        public List<CellDisplayModel> GetCellsOfStorage(int storageId, string email)
        {
            var storage = _storageRepository.GetStorageById(storageId);
            if (storage == null || storage.Person.Email != email) return null;
            var list = Mapper.Map<List<CellDisplayModel>>(storage.Cells.ToList());
            foreach (var item in list)
                item.SafetyStatus = GetSafetyStatusByDatetime(item.BestBefore);
            return list;
        }

        private Safety GetSafetyStatusByDatetime(DateTime? bestBefore)
        {
            if (bestBefore == null) return Safety.Unknown;
            var days = (int)Math.Floor((bestBefore.Value.Date - DateTime.UtcNow.Date).TotalDays);
            return days > 1 ? Safety.IsSafe
                : days > 0 ? Safety.ExpiresTomorrow
                : days == 0 ? Safety.ExpiresToday
                : Safety.Expired;
        }
    }
}