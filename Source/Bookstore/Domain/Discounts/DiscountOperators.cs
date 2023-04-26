namespace Bookstore.Domain.Discounts;

public static class DiscountOperators
{
    public static IDiscount Then(this IDiscount first, IDiscount second) =>
        first is ChainedDiscounts chain ? chain.Then(second)
        : ChainedDiscounts.Create(first, second);
}