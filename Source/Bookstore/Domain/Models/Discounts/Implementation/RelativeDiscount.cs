using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models.Discounts.Implementation;

public class RelativeDiscount : SingleDiscount
{
    public decimal Factor { get; }

    public RelativeDiscount(decimal factor) : base($"Discount {factor:P2}")
    {
        if (factor < 0 || factor > 1)
            throw new ArgumentOutOfRangeException(nameof(factor), "Discount factor must be between 0 and 1");

        this.Factor = factor;
    }

    protected override Money ApplySingle(Money price) => price * this.Factor;
}