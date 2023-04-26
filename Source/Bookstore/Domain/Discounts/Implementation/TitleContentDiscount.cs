using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public class TitleContentDiscount : SingleDiscount
{
    private decimal Factor { get; }
    private string Contains { get; }

    public TitleContentDiscount(decimal factor, string contains) : base($"{factor:P2} off titles containing \"{contains}\"") =>
        (Factor, Contains) = (factor, contains);

    protected override Money GetDiscountAmount(Money price) => price * Factor;

    public override IDiscount Within(DiscountContext context) =>
        (context.book?.Title.Contains(this.Contains, StringComparison.InvariantCultureIgnoreCase) ?? false) ? this : new NoDiscount();

    public override string ToString() =>
        $"{this.Factor:P2} if title contains \"{this.Contains}\"";
}