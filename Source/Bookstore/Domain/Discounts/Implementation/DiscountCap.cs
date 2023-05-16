using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class DiscountCap : IDiscount
{
    private decimal CapFactor { get; }
    private IDiscount Discount { get; }

    public DiscountCap(decimal capFactor, IDiscount discount) =>
        (CapFactor, Discount) = (capFactor, discount);

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price)
    {
        Money remainingDiscount = price * CapFactor;
        foreach (var discount in Discount.GetDiscountAmounts(price))
        {
            int comparison = discount.Amount.CompareTo(remainingDiscount);

            if (comparison >= 0)
            {
                yield return discount with { Amount = remainingDiscount, Label = comparison > 0 ? $"{discount.Label} (capped)" : discount.Label };
                yield break;
            }

            yield return discount;
            remainingDiscount -= discount.Amount;
        }
    }

    public IDiscount Within(DiscountContext context) =>
        new DiscountCap(CapFactor, Discount.Within(context));

    public override string ToString() =>
        $"[{CapFactor:P2} cap on {Discount}]";
}