﻿using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using System.Linq;

namespace SmartKitchen.DomainService.Profiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<Basket, BasketDisplayModel>()
                .ForMember(dest => dest.Products, opts => { opts.MapFrom(from => from.BasketProducts.Count); })
                .ForMember(dest => dest.BoughtProducts, opts => { opts.MapFrom(from => from.BasketProducts.Count(x => x.Bought)); })
                .ForMember(dest => dest.FullPrice, opts => { opts.MapFrom(
                    from => from.BasketProducts.Any() ? from.BasketProducts.Sum(x => x.Price) : 0); });
            CreateMap<BasketProduct, BasketProductDisplayModel>()
                .ForMember(dest => dest.ProductName, opts => { opts.MapFrom(from => from.Cell.Product.Name); })
                .ForMember(dest => dest.StorageName, opts => { opts.MapFrom(from => from.Cell.Storage.Name); });
        }
    }
}
