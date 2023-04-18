namespace Bookstore.Domain.Common;

public record struct Money(decimal Amount, Currency Currency)
{
    public override string ToString() => $"{Amount:0.00} {Currency.Symbol}";
}

public record struct Currency(string Symbol)
{
    public override string ToString() => Symbol;
}