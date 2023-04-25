using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models.Discounts;

public abstract class SingleDiscount : IDiscount
{
    private string Label { get; }

    protected SingleDiscount(string label) =>
        Label = label ?? string.Empty;

    protected abstract Money ApplySingle(Money price);

    public IEnumerable<DiscountApplication> ApplyTo(Money price) =>
        new DiscountApplication[] { new(this.Label, this.ApplySingle(price)) };
}