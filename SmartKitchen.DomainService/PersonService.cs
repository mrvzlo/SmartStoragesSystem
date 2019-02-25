using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using SmartKitchen.Domain.DisplayModel;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;
using SmartKitchen.Domain.IService;
using SmartKitchen.Domain.Responses;
using SmartKitchen.Models;

namespace SmartKitchen.DomainService
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

        public List<StorageDescription> GetMyStoragesWithDescription(string email)
        {
            var person = _personRepository.GetPersonByEmail(email);
            return _storageRepository.GetAllUserStorages(person.Id).ProjectTo<StorageDescription>(MapperConfig).ToList();
        }

        public Response IsOwner(Storage s, Person p)
        {
            if (s == null || p == null) return new Response(404);
            if (s.Owner != p.Id) return new Response(403);
            return Response.Success();
        }
        public Response IsOwner(Basket b, Person p)
        {
            if (b == null || p == null) return new Response(404);
            if (b.Owner != p.Id) return new Response(403);
            return Response.Success();
        }
    }
}
