using Bookstore.Data;
using Bookstore.Data.Seeding;
using Bookstore.Data.Specifications;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BooksModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IUnitOfWork _dbContext;
    public IEnumerable<Book> Books { get; private set; } = Enumerable.Empty<Book>();
    public IReadOnlyList<string> PublishedAuthorInitials { get; private set; } = Array.Empty<string>();
    private readonly IDataSeed<Book> _booksSeed;
    private readonly ISpecification<Book> _spec;

    public BooksModel(ILogger<IndexModel> logger, IUnitOfWork dbContext, IDataSeed<Book> booksSeed, ISpecification<Book> spec) =>
        (_logger, _dbContext, _booksSeed, _spec) = (logger, dbContext, booksSeed, spec);

    public async Task OnGet([FromQuery] string? initial)
    {
        await this._booksSeed.SeedAsync();
        await this.PopulatePublishedAuthorInitials();
        await this.PopulateBooks(initial);
    }

    private async Task PopulatePublishedAuthorInitials() =>
        this.PublishedAuthorInitials  = await ((BookstoreDbContext)_dbContext).BookAuthors      // Requires redefinition when specifications are implemented
            .GetPublishedAuthors()
            .Select(author => author.LastName.Substring(0, 1))
            .Distinct()
            .OrderBy(initial => initial)
            .ToListAsync();

    private async Task PopulateBooks(string? authorInitial)
    {
        ISpecification<Book> spec = authorInitial is null ? _spec : _spec.ByAuthorInitial(authorInitial);
        this.Books = await _dbContext.Books.QueryAsync(spec.OrderByTitle());
    }
}
