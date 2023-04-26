using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class RelativeDiscount : SingleDiscount
{
    public decimal Factor { get; }

    public RelativeDiscount(decimal factor) : base($"{factor:P2} off price")
    {
        if (factor <= 0 || factor > 1)
            throw new ArgumentOutOfRangeException(nameof(factor), "Discount factor must be between 0 (exclusive) and 1 (inclusive)");

        this.Factor = factor;
    }

    protected override Money GetDiscountAmount(Money price) => price * this.Factor;

    public override string ToString() =>
        $"Relative discount {this.Factor:P2}";
}