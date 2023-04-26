using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public record NoDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price) => Enumerable.Empty<DiscountApplication>();
}