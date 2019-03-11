using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.DomainService.Profiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageDisplayModel>()
                .ForMember(dest => dest.CellCount, opts => { opts.MapFrom(from => from.Cells.Count); });

            CreateMap<StorageType, StorageTypeDisplayModel>();
        }
    }
}
