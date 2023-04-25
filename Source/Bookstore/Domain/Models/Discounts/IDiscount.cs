using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models.Discounts;

public interface IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Money price);
}

public record DiscountApplication(string Label, Money Amount);