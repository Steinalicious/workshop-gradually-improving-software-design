using System.Globalization;
using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class TitlePrefixDiscount : SingleDiscount
{
    private decimal Factor { get; }
    private string TitlePrefix { get; }

    public TitlePrefixDiscount(decimal factor, string titlePrefix) : base($"{factor:P2} off titles beginning with \"{titlePrefix}\"") =>
        (Factor, TitlePrefix) = (factor, titlePrefix);

    protected override Money GetDiscountAmount(Money price) => price * Factor;

    public override IDiscount Within(DiscountContext context) =>
        (context.book?.Title.StartsWith(this.TitlePrefix, true, CultureInfo.InvariantCulture) ?? false) ? this : new NoDiscount();

    public override string ToString() =>
        $"{this.Factor:P2} if title begins with \"{this.TitlePrefix}\"";
}