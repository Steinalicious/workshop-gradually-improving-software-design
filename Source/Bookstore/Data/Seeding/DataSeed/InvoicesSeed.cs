using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding.DataSeed;

public class InvoicesSeed : IDataSeed<Invoice>
{
    private readonly BookstoreDbContext _context;

    public InvoicesSeed(BookstoreDbContext context) => _context = context;

    public Task<Invoice> EnsureEqualExists(Invoice entity) => Task.FromResult(entity);

    public async Task SeedAsync()
    {
        if (await this._context.Invoices.AnyAsync()) return;
    }
}
