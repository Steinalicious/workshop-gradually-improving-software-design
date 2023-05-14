namespace Bookstore.Common;

public static class OptionalExtensions
{
    public static Option<T> AsOption<T>(this T? value) where T : class =>
        value is null ? Option<T>.None() : Option<T>.Some(value);

    public static ValueOption<T> AsOption<T>(this T? value) where T : struct =>
        value is null ? ValueOption<T>.None() : ValueOption<T>.Some(value.Value);
}