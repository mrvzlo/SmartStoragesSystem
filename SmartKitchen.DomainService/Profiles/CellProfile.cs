using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.DomainService.Profiles
{
    public class CellProfile : Profile
    {
        public CellProfile()
        {
            CreateMap<BasketProductCreationModel, CellCreationModel>();
            CreateMap<Cell, CellDisplayModel>()
                .ForMember(dest => dest.ProductName, opts => { opts.MapFrom(from => from.Product.Name); })
                .ForMember(dest => dest.CategoryName, opts => { opts.MapFrom(from => from.Product.Category.Name); })
                .ForMember(dest => dest.CellChanges, opts => { opts.MapFrom(from => from.CellChanges); })
                .ForMember(dest => dest.SafetyStatus, opt => opt.Ignore())
                .ForMember(dest => dest.AmountStatus, opt => opt.Ignore());
        }
    }
}
