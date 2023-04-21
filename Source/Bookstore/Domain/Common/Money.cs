namespace Bookstore.Domain.Common;

public record struct Money(decimal Amount, Currency Currency)
{
    public override string ToString() => $"{Amount:0.00} {Currency.Symbol}";
}

public record struct Currency(string Symbol)
{
    public static Currency USD { get; } = new("USD");
    public static Currency EUR { get; } = new("EUR");

    public override string ToString() => Symbol;
}