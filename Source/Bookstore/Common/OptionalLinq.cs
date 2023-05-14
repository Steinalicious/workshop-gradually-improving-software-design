namespace Bookstore.Common;

public static class OptionalLinq
{
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source) where T : class =>
        source.FirstOrDefault().AsOption();

    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source) where T : class =>
        source.SingleOrDefault().AsOption();
}