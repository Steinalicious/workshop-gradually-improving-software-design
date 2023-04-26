using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public interface IDiscount
{
    IEnumerable<DiscountApplication> GetDiscountAmounts(Money price);
    public IDiscount Within(DiscountContext context) => this;
}

public record DiscountApplication(string Label, Money Amount);