using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public class RelativeDiscount : IDiscount
{
    public decimal MultiplyingFactor { get; }

    public RelativeDiscount(decimal multiplyingFactor) =>
        MultiplyingFactor = multiplyingFactor > 0 && multiplyingFactor < 1
        ? multiplyingFactor : throw new ArgumentOutOfRangeException(nameof(multiplyingFactor));

    public override string ToString() => $"Relative discount: {MultiplyingFactor * 100:0.00}%";

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price)
    {
        yield return new($"Relative discount {MultiplyingFactor * 100:0.00}%", price * MultiplyingFactor);
    }

    public IDiscount Within(DiscountContext context) => this;
}
