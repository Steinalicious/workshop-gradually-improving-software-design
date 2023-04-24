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
    private readonly IDataSeed<Book> _booksSeed;

    public BooksModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDataSeed<Book> booksSeed)
    {
        _logger = logger;
        _dbContext = dbContext;
        _booksSeed = booksSeed;
    }

    public async Task OnGet()
    {
        await this._booksSeed.SeedAsync();
        this.Books = await _dbContext.Books.ToListAsync();
    }
}
