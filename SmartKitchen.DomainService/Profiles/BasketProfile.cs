using AutoMapper;
using SmartKitchen.Domain.DisplayModel;
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
                .ForMember(dest => dest.Products, opts => { opts.MapFrom(from => from.BasketProducts.Count(x => x.Bought)); });
        }
    }
}
