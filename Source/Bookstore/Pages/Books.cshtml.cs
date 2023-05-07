using Bookstore.Data.Seeding;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BooksModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    public IEnumerable<Book> Books { get; private set; } = Enumerable.Empty<Book>();
    public IReadOnlyList<string> PublishedAuthorInitials { get; private set; } = Array.Empty<string>();
    private readonly IDataSeed<Book> _booksSeed;

    public BooksModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDataSeed<Book> booksSeed)
    {
        _logger = logger;
        _dbContext = dbContext;
        _booksSeed = booksSeed;
    }

    public async Task OnGet([FromQuery] string? initial)
    {
        await this._booksSeed.SeedAsync();
        await this.PopulatePublishedAuthorInitials();
        await this.PopulateBooks(initial);
    }

    private async Task PopulatePublishedAuthorInitials() =>
        this.PublishedAuthorInitials  = await _dbContext.BookAuthors
            .GetPublishedAuthors()
            .Select(author => author.LastName.Substring(0, 1))
            .Distinct()
            .OrderBy(initial => initial)
            .ToListAsync();

    private async Task PopulateBooks(string? authorInitial)
    {
        IQueryable<Book> allBooks = _dbContext.Books.Include(book => book.AuthorsCollection).ThenInclude(bookAuthor => bookAuthor.Person);

        IQueryable<Book> books = authorInitial is null ? allBooks
        : allBooks.Where(book => book.AuthorsCollection.Any(author => author.Person.LastName.StartsWith(authorInitial)));

        this.Books = await books.OrderBy(book => book.Title).ToListAsync();
    }
}
