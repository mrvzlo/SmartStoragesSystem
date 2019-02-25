using AutoMapper;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.DomainService.Profiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageDescription>()
                .ForMember(dest => dest.CellCount, opts => { opts.MapFrom(from => from.Cells.Count); });

            CreateMap<StorageType, StorageTypeDisplayModel>();
        }
    }
}
