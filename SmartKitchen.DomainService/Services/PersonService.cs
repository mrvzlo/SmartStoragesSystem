using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.Enums;
using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;
using SmartKitchen.Domain.Responses;

namespace SmartKitchen.DomainService.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IStorageRepository _storageRepository;

        public PersonService(IPersonRepository personRepository, IStorageRepository storageRepository)
        {
            _personRepository = personRepository;
            _storageRepository = storageRepository;
        }

        public ServiceResponse IsOwner(Storage s, Person p)
        {
            var response = new ServiceResponse();
            if (s == null || p == null) response.Errors.Add(new ModelStateError("", AccessError.StorageNotFound));
            else if (s.Owner != p.Id) response.Errors.Add(new ModelStateError("", AccessError.NoPermission));
            return response;
        }
        public ServiceResponse IsOwner(Basket b, Person p)
        {
            var response = new ServiceResponse();
            if (b == null || p == null) response.Errors.Add(new ModelStateError("", AccessError.BasketNotFound));
            else if (b.Owner != p.Id) response.Errors.Add(new ModelStateError("", AccessError.NoPermission));
            return response;
        }
    }
}
