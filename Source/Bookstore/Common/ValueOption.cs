namespace Bookstore.Common;

public readonly record struct ValueOption<T> where T : struct
{
    private readonly T? _value;

    private ValueOption(T? value) => _value = value;

    public static ValueOption<T> Some(T value) => new(value);
    public static ValueOption<T> None() => new(null);

    public ValueOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : struct =>
        _value is null ? new ValueOption<TResult>() : new ValueOption<TResult>(map(_value.Value));

    public ValueOption<TResult> MapOptional<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
        _value is null ? new ValueOption<TResult>() : map(_value.Value);

    public Option<TResult> MapToObject<TResult>(Func<T, TResult> map) where TResult : class =>
        _value is null ? Option<TResult>.None() : Option<TResult>.Some(map(_value.Value));

    public Option<TResult> MapOptionalToObject<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
        _value is null ? new Option<TResult>() : map(_value.Value);

    public ValueOption<T> Audit(Action<T> audit)
    {
        if (_value is not null) audit(_value.Value);
        return this;
    }

    public T Reduce(T whenNone) => _value ?? whenNone;
    public T Reduce(Func<T> whenNone) => _value ?? whenNone();
}