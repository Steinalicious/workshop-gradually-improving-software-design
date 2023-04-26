using System.Collections.Immutable;
using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class ChainedDiscounts : IDiscount
{
    private ImmutableList<IDiscount> Discounts { get; }

    public static IDiscount Create(params IDiscount[] discounts) =>
        discounts.Any(discount => discount is ChainedDiscounts) ? FlattenChainedAndChain(discounts)
        : discounts.Any(discount => discount is NoDiscount) ? RemoveNoDiscountAndChain(discounts)
        : discounts.Length == 0 ? new NoDiscount()
        : discounts.Length == 1 ? discounts[0]
        : new ChainedDiscounts(discounts.ToImmutableList());

    private static IDiscount RemoveNoDiscountAndChain(IEnumerable<IDiscount> discounts) =>
        Create(discounts.Where(discount => discount is not NoDiscount).ToArray());

    private static IDiscount FlattenChainedAndChain(IEnumerable<IDiscount> discounts) =>
        Create(discounts.SelectMany(discount => discount is ChainedDiscounts chained ? chained.Discounts : (IEnumerable<IDiscount>)new[] { discount }).ToArray());

    private ChainedDiscounts(ImmutableList<IDiscount> discounts) =>
        Discounts = discounts;

    public ChainedDiscounts Then(IDiscount next) =>
        next is NoDiscount ? this
        : next is ChainedDiscounts chained ? new(Discounts.AddRange(chained.Discounts))
        : new(Discounts.Add(next));

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price)
    {
        foreach (IDiscount discount in this.Discounts)
        {
            IEnumerable<DiscountApplication> singleApplications = discount.GetDiscountAmounts(price);
            foreach (DiscountApplication discountApplication in singleApplications)
            {
                DiscountApplication applied = discountApplication.Amount.CompareTo(price) <= 0 ? discountApplication
                : discountApplication with { Amount = price };
                yield return applied;
                price -= applied.Amount;
                if (price.Amount == 0) yield break;
            }
        }
    }

    public IDiscount Within(DiscountContext context) =>
        Create(this.Discounts.Select(discount => discount.Within(context)).ToArray());

    public override string ToString() =>
        $"[{string.Join(", then ", this.Discounts)}]";
}