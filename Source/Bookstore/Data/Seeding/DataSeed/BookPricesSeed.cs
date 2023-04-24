using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding;

public class BookPricesSeed : IDataSeed<BookPrice>
{
    private readonly BookstoreDbContext _dbContext;

    public BookPricesSeed(BookstoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<BookPrice> EnsureEqualExists(BookPrice entity)
    {
        throw new NotImplementedException();
    }

    public Task SeedAsync()
    {
        throw new NotImplementedException();
    }
}