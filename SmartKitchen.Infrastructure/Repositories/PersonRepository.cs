using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;
using System;
using System.Linq;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Register(Person person)
        {
            _dbContext.People.Add(person);
            _dbContext.SaveChanges();
        }

        public Person GetPersonByEmail(string email) =>
            _dbContext.People.FirstOrDefault(x => x.Email == email);

        public Person GetPersonByName(string name) =>
            _dbContext.People.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
