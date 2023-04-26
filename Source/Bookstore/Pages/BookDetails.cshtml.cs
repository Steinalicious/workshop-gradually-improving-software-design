using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    public record PriceLine(string Label, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    private readonly IDataSeed<BookPrice> _bookPricesSeed;
    private IDiscount Discount { get; set; }

    public Book Book { get; private set; } = null!;

    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = new List<PriceLine>();

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDataSeed<BookPrice> bookPricesSeed, IDiscount discount, IDataSeed<BookPrice> bookPricesSeed2)
    {
        _logger = logger;
        _dbContext = dbContext;
        _bookPricesSeed = bookPricesSeed;
        Discount = discount;
        _bookPricesSeed = bookPricesSeed2;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        await _bookPricesSeed.SeedAsync();
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            
            this.Book = book;
            this.Discount = this.Discount.Within(new DiscountContext(book));

            Money? originalPrice = (await _dbContext.BookPrices.For(book).At(DateTime.Now).FirstOrDefaultAsync())?.Price;
            this.PriceSpecification = originalPrice.HasValue ? this.CalculatePriceLines(originalPrice.Value) : new List<PriceLine>();

            return Page();
        }

        return Redirect("/books");
    }

    private List<PriceLine> CalculatePriceLines(Money originalPrice)
    {
        List<PriceLine> priceLines = new();

        _logger.LogInformation("Applying discount: {discount}", Discount);

        priceLines.Add(new("Original price", originalPrice));
        priceLines.AddRange(Discount.GetDiscountAmounts(originalPrice).Select(a => new PriceLine(a.Label, a.Amount)));

        if (priceLines.Count > 1)
        {
            PriceLine finalPrice = priceLines.Aggregate((price, discount) => price with { Amount = price.Amount - discount.Amount }) with { Label = "Final price" };
            priceLines.Add(finalPrice);
        }

        return priceLines;
    }
}
