namespace Bookstore.Common;

public static class ValueOptionalLinq
{
    public static ValueOption<T> FirstOrNone<T>(this IEnumerable<T> source) where T : struct
    {
        using IEnumerator<T> enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return ValueOption<T>.None();
        return ValueOption<T>.Some(enumerator.Current);
    }

    public static ValueOption<T> SingleOrNone<T>(this IEnumerable<T> source) where T : struct
    {
        using IEnumerator<T> enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return ValueOption<T>.None();
        T result = enumerator.Current;
        if (enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains more than one element");
        return ValueOption<T>.Some(result);
    }
}
