using System.Globalization;
using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class TitlePrefixDiscount : IDiscount
{
    private decimal Factor { get; }
    private string TitlePrefix { get; }

    public TitlePrefixDiscount(decimal factor, string titlePrefix) =>
        (Factor, TitlePrefix) = (factor, titlePrefix);

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price)
    {
        yield return new DiscountApplication($"{Factor:P2} off titles beginning with \"{TitlePrefix}\"", price * Factor);
    }

    public IDiscount Within(DiscountContext context) =>
        (context.book?.Title.StartsWith(this.TitlePrefix, true, CultureInfo.InvariantCulture) ?? false) ? this : new NoDiscount();

    public override string ToString() =>
        $"{this.Factor:P2} if title begins with \"{this.TitlePrefix}\"";
}