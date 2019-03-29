using AutoMapper;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.Enitities;
using System;
using System.Linq;

namespace SmartKitchen.DomainService.Profiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageDisplayModel>()
                .ForMember(dest => dest.CellCount, opts => { opts.MapFrom(from => from.Cells.Count); })
                .ForMember(dest => dest.Expired, opts => {
                opts.MapFrom(from => from.Cells.Count(x =>
                    x.BestBefore != null && (int)Math.Floor((x.BestBefore.Value.Date - DateTime.Now.Date).TotalDays) < 0));
                })
                .ForMember(dest => dest.Absent, opts => {
                opts.MapFrom(from => from.Cells.Count(x =>
                    x.CellChanges.OrderByDescending(c => c.UpdateDate).FirstOrDefault().Amount == 0));
                });
            CreateMap<StorageType, StorageTypeDisplayModel>();
        }
    }
}
