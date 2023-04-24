namespace Bookstore.Domain.Common;

public record struct Money : IComparable<Money>
{
    public override string ToString() => $"{Amount:0.00} {Currency.Symbol}";

    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    public Money() : this(0, Currency.Empty) { }

    public Money(decimal amount, Currency currency)
    {
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money of different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency) throw new InvalidOperationException("Cannot subtract money of different currencies");
        if (Amount < other.Amount) throw new InvalidOperationException("Cannot subtract more money than available");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Scale(decimal factor)
    {
        if (factor < 0) throw new InvalidOperationException("Cannot multiply by a negative factor");
        return new Money(Amount * factor, Currency);
    }

    public int CompareTo(Money other) =>
        Currency == other.Currency ? Amount.CompareTo(other.Amount)
        : throw new InvalidOperationException("Cannot compare money of different currencies");

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, decimal right) => left.Scale(right);
    public static Money operator *(decimal left, Money right) => right.Scale(left);
}

public record struct Currency(string Symbol)
{
    public override string ToString() => Symbol;

    public Currency() : this(string.Empty) { }

    internal static Currency Empty => new(string.Empty);

    public static readonly Currency EUR = new("EUR");
    public static readonly Currency USD = new("USD");

    public Money Amount(decimal amount) => new(amount, this);
}