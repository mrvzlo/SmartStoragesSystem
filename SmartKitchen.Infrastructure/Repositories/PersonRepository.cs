using SmartKitchen.Domain.Enitities;
using System;
using System.Linq;
using SmartKitchen.Domain.IRepositories;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext) => 
            _dbContext = dbContext;

        public void RegisterOrUpdate(Person person) => 
            _dbContext.InsertOrUpdate(person);

        public Person GetPersonByEmail(string email) =>
            _dbContext.People.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public Person GetPersonByName(string name) =>
            _dbContext.People.FirstOrDefault(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        
        public Person GetPersonById(int id) =>
            _dbContext.People.Find(id);
    }
}
