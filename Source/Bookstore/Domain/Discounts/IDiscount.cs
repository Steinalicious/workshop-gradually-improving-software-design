using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public interface IDiscount
{
    IEnumerable<DiscountApplication> GetDiscountAmounts(Money price);
    IDiscount Within(DiscountContext context);
}

public record DiscountApplication(string Label, Money Amount);