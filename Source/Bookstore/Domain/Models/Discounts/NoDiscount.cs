using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models.Discounts;

public record NoDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Money price) => Enumerable.Empty<DiscountApplication>();
}