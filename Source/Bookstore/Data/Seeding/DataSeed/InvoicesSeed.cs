using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Seeding.DataSeed;

public class InvoicesSeed : IDataSeed<Invoice>
{
    private readonly ILogger<InvoicesSeed> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<Book> _booksSeed;
    private readonly IDataSeed<BookPrice> _pricesSeed;
    private readonly IDataSeed<Customer> _customersSeed;

    public InvoicesSeed(ILogger<InvoicesSeed> logger, BookstoreDbContext context, IDataSeed<Book> booksSeed, IDataSeed<BookPrice> pricesSeed, IDataSeed<Customer> customersSeed) =>
        (_logger, _context, _booksSeed, _pricesSeed, _customersSeed) = (logger, context, booksSeed, pricesSeed, customersSeed);

    public Task<Invoice> EnsureEqualExists(Invoice entity) => Task.FromResult(entity);

    public async Task SeedAsync()
    {
        if (await this._context.Invoices.AnyAsync()) return;

        await this._booksSeed.SeedAsync();
        await this._pricesSeed.SeedAsync();
        await this._customersSeed.SeedAsync();

        (Book book, Money price)[] bookPrices = (await this.LoadBookPricesAsync()).ToArray();
        Customer[] customers = await this._context.Customers.OrderBy(customer => customer.Label).Take(5).ToArrayAsync();

        int invoicesCount = 20;
        Random rand = new(42);

        for (int i = 0; i < invoicesCount; i++)
        {
            Invoice invoice = Invoice.CreateNew(
                customers[rand.Next(0, customers.Length)],
                DateTime.Now.AddDays(-rand.Next(0, 40)),
                rand.Next(10, 60));

            _context.Invoices.Add(invoice);

            int booksCount = rand.Next(1, 5);
            for (int j = 0; j < booksCount; j++)
            {
                (Book book, Money price) = bookPrices[rand.Next(0, bookPrices.Length)];
                InvoiceLine line = invoice.Add(book, price);
                _context.InvoiceLines.Add(line);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task<IEnumerable<(Book book, Money price)>> LoadBookPricesAsync() =>
        (await this._context.Books
            .GetBooks()
            .Join(this._context.BookPrices, b => b.Id, bp => bp.BookId, (b, bp) => new { Book = b, Price = bp.Price })
            .OrderBy(row => row.Book.Title)
            .ToListAsync())
        .Select(row => (row.Book, row.Price));
}
