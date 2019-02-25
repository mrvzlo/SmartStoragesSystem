using System.Linq;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepository;

namespace SmartKitchen.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Person GetPersonByEmail(string email) =>
            _dbContext.People.FirstOrDefault(x => x.Email == email);
    }
}
