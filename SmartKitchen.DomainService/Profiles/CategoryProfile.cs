using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;

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
