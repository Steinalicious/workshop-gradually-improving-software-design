namespace Bookstore.Domain.Common;

public class Currency
{
    public string Symbol { get; }

    public Currency(string symbol) => Symbol = symbol;

    public override string ToString() => Symbol;

    public Money Amount(decimal amount) => new(amount, this);

    public static Currency Empty => new(string.Empty);

    public static Currency USD => new("USD");
    public static Currency EUR => new("EUR");
    public static Currency GBP => new("GBP");
}