using Bookstore.Domain.Common;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    public record PriceLine(string Label, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    public Book Book { get; private set; } = null!;

    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = Array.Empty<PriceLine>();

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext) =>
        (_logger, _dbContext) = (logger, dbContext);

    public async Task<IActionResult> OnGet(Guid id)
    {
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            this.Book = book;
            this.PriceSpecification = new List<PriceLine>() { new("Price", BookPricing.SeedPriceFor(book, Currency.USD).Value) };
            return Page();
        }

        return Redirect("/books");
    }
}
