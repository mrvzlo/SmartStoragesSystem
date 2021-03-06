﻿using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.Domain.IServices
{
    public interface IProductService
    {
        Product GetOrAddAndGetProduct(string name);
        Product GetProductByName(string name);
        IQueryable<ProductDisplayModel> GetAllProductDisplays();
        ItemCreationResponse AddProduct(NameCreationModel model);
        int UpdateProductList(List<ProductDisplayModel> list);
        IQueryable<string> GetProductNamesByStart(string start);
    }
}
