using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    public Book Book { get; private set; } = null!;

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            this.Book = book;
            return Page();
        }

        return Redirect("/books");
    }
}
