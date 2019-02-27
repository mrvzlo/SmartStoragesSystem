using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using System.Linq;

namespace SmartKitchen.DomainService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDisplayModel>()
                .ForMember(dest => dest.CategoryName, opts => { opts.MapFrom(from => from.Category.Name); })
                .ForMember(dest => dest.Usages, opts => { opts.MapFrom(from => from.Cells.Count); });

        }
    }
}
