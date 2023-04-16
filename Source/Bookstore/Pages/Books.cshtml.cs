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

    public BooksModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task OnGet()
    {
        this.Books = await _dbContext.Books.GetBooks().ToListAsync();
    }
}
