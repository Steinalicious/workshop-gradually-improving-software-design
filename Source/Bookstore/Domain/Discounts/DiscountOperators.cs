using Bookstore.Domain.Discounts.Implementation;

namespace Bookstore.Domain.Discounts;

public static class DiscountOperators
{
    public static IDiscount Then(this IDiscount first, IDiscount second) =>
        ChainedDiscounts.Create(first, second);

    public static IDiscount And(this IDiscount first, IDiscount second) =>
        ParallelDiscounts.Create(first, second);

    public static IDiscount CapTo(this IDiscount discount, decimal factor) =>
        new DiscountCap(factor, discount);
}