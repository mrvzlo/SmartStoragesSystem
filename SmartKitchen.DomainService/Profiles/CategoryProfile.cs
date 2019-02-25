using AutoMapper;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Models;

namespace SmartKitchen.DomainService.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDisplay>()
                .ForMember(dest => dest.ProductsCount, opts => { opts.MapFrom(from => from.Products.Count); });            
        }
    }
}
