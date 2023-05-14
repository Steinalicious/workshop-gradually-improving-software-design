namespace Bookstore.Common;

public readonly record struct Option<T> where T : class
{
    private readonly T? _value;

    public Option() : this(null) { }
    private Option(T? value) => _value = value;

    public static Option<T> Some(T value) => new(value);
    public static Option<T> None() => new(null);

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        _value is null ? new Option<TResult>() : new Option<TResult>(map(_value));

    public Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
        _value is null ? new Option<TResult>() : map(_value);

    public ValueOption<TResult> MapToValue<TResult>(Func<T, TResult> map) where TResult : struct =>
        _value is null ? ValueOption<TResult>.None() : ValueOption<TResult>.Some(map(_value));

    public ValueOption<TResult> MapOptionalToValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
        _value is null ? new ValueOption<TResult>() : map(_value);

    public Option<T> Audit(Action<T> audit)
    {
        if (_value is not null) audit(_value);
        return this;
    }

    public T Reduce(T whenNone) => _value ?? whenNone;
    public T Reduce(Func<T> whenNone) => _value ?? whenNone();

    public static implicit operator T?(Option<T> option) => option._value;
}
