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
    private readonly IDiscount _discount;
    public Book Book { get; private set; } = null!;

    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = Array.Empty<PriceLine>();

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDiscount discount) =>
        (_logger, _dbContext, _discount) = (logger, dbContext, discount);

    public async Task<IActionResult> OnGet(Guid id)
    {
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            this.Book = book;
            Money originalPrice = BookPricing.SeedPriceFor(book, Currency.USD).Value;
            this.PriceSpecification = CalculatePriceLines(book, originalPrice);
            return Page();
        }

        return Redirect("/books");
    }

    private List<PriceLine> CalculatePriceLines(Book book, Money originalPrice)
    {
        _logger.LogInformation("Applying discount: {discount}", _discount);

        List<PriceLine> discountLines = _discount
            .Within(new(book))
            .GetDiscountAmounts(originalPrice).Select(a => new PriceLine(a.Label, a.Amount))
            .ToList();

        if (!discountLines.Any()) return new() { new("Price", originalPrice) };

        List<PriceLine> priceLines = new()
        {
            new("Original price", originalPrice)
        };
        priceLines.AddRange(discountLines);

        PriceLine finalPrice = priceLines.Aggregate((price, discount) => price with { Amount = price.Amount - discount.Amount }) with { Label = "Final price" };
        priceLines.Add(finalPrice);

        return priceLines;
    }
}
