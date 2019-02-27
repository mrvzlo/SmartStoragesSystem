using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.DisplayModels;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;
using System.Collections.Generic;
using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class StorageTypeService : BaseService, IStorageTypeService
    {
        private readonly IStorageTypeRepository _storageTypeRepository;

        public StorageTypeService(IStorageTypeRepository storageTypeRepository)
        {
            _storageTypeRepository = storageTypeRepository;
        }

        public IQueryable<StorageType> GetAllStorageTypes() =>
            _storageTypeRepository.GetAllStorageTypes();

        public bool ExistsWithId(int id) =>
            _storageTypeRepository.GetStorageTypeById(id) != null;
    }
}
