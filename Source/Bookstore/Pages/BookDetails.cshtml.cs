using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    private readonly IDataSeed<BookPrice> _bookPricesSeed;

    public Book Book { get; private set; } = null!;
    public Money? Price { get; private set; } = null;

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDataSeed<BookPrice> bookPricesSeed)
    {
        _logger = logger;
        _dbContext = dbContext;
        _bookPricesSeed = bookPricesSeed;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        await _bookPricesSeed.SeedAsync();
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            this.Book = book;
            this.Price = (await _dbContext.BookPrices.For(book).At(DateTime.Now).FirstOrDefaultAsync())?.Price;
            return Page();
        }

        return Redirect("/books");
    }
}
