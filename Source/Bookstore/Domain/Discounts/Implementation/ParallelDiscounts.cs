using System.Collections.Immutable;
using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class ParallelDiscounts : IDiscount
{
    private ImmutableList<IDiscount> Discounts { get; }
    
    private ParallelDiscounts(ImmutableList<IDiscount> discounts) => Discounts = discounts;

    public static IDiscount Create(params IDiscount[] discounts) =>
        discounts.Any(discount => discount is NoDiscount) ? Create(discounts.Where(discount => discount is not NoDiscount).ToArray())
        : discounts.Length == 0 ? new NoDiscount()
        : discounts.Length == 1 ? discounts[0]
        : new ParallelDiscounts(discounts.ToImmutableList());

    public IDiscount And(IDiscount next) =>
        next is NoDiscount ? this
        : next is ParallelDiscounts parallel ? new(Discounts.AddRange(parallel.Discounts))
        : new(Discounts.Add(next));
    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price)
    {
        Money remaining = price;
        foreach (var discountApplication in Discounts.SelectMany(discount => discount.GetDiscountAmounts(price)))
        {
            DiscountApplication applied = discountApplication.Amount.CompareTo(remaining) <= 0 ? discountApplication
            : discountApplication with { Amount = remaining };
            yield return applied;
            remaining -= applied.Amount;
            if (remaining.Amount == 0) yield break;
        }
    }

    public IDiscount Within(DiscountContext context) =>
        Create(Discounts.Select(discount => discount.Within(context)).ToArray());

    public override string ToString() =>
        $"[{string.Join(", and ", this.Discounts)}]";
}