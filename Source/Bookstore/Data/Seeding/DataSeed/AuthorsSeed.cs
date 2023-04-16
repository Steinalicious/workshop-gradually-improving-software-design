using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;

namespace Bookstore.Data.Seeding.DataSeed;

public class AuthorsSeed : IDataSeed<Person>
{
    private readonly BookstoreDbContext _dbContext;

    public AuthorsSeed(BookstoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedAsync() => Task.CompletedTask;

    public Task<Person> EnsureEqualExists(Person entity)
    {
        if (_dbContext.People.GetPeople().ByName(entity.FirstName, entity.LastName).FirstOrDefault() is Person author)
        {
            return Task.FromResult(author);
        }

        _dbContext.People.Add(entity);
        _dbContext.SaveChanges();
        return Task.FromResult(entity);
    }
}