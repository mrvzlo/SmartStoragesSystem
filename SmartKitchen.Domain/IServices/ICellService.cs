﻿using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;
using System;
using System.Linq;

namespace SmartKitchen.Domain.IServices
{
    public interface ICellService
    {
        Cell GetOrAddAndGetCell(CellCreationModel model, string email);
        ItemCreationResponse AddCell(CellCreationModel model, string email);
        ItemCreationResponse AddCell(CellCreationModel model, Person person);
        ServiceResponse DeleteCellById(int id, string email);
        ServiceResponse DeleteCellById(int id, Person person);
        void DeleteCell(Cell cell);
        IQueryable<CellDisplayModel> GetCellsOfStorage(int storageId, string email);
        ServiceResponse UpdateCellBestBefore(int id, DateTime? value, string email);
        ServiceResponse UpdateCellBestBefore(int id, DateTime? value, Person person);
        ServiceResponse UpdateCellAmount(int id, int value, string email);
        ServiceResponse UpdateCellAmount(int id, int value, Person person);
        ItemCreationResponse MoveBasketProductToStorage(BasketProduct basketProduct, Basket basket, Person person);
    }
}
