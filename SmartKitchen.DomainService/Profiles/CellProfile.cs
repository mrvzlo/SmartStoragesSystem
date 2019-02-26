using AutoMapper;
using SmartKitchen.Domain.CreationModels;

namespace SmartKitchen.DomainService.Profiles
{
    public class CellProfile : Profile
    {
        public CellProfile()
        {
            CreateMap<BasketProductCreationModel, CellCreationModel>();
        }
    }
}
