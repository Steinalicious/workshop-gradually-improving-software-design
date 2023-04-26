using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public abstract class SingleDiscount : IDiscount
{
    private string Label { get; }

    protected SingleDiscount(string label) =>
        Label = label ?? string.Empty;

    protected abstract Money GetDiscountAmount(Money price);

    public virtual IDiscount Within(DiscountContext context) => this;

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price) =>
        new DiscountApplication[] { new(this.Label, this.GetDiscountAmount(price)) };
}