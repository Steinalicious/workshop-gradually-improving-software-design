namespace Bookstore.Domain.Discounts;

public class RelativeDiscount
{
    public decimal MultiplyingFactor { get; }

    public RelativeDiscount(decimal multiplyingFactor) =>
        MultiplyingFactor = multiplyingFactor >= 0 ? multiplyingFactor : throw new ArgumentOutOfRangeException(nameof(multiplyingFactor));
}
