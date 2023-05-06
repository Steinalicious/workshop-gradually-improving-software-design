namespace Demo;

public static class EnumerableExtensions
{
    public static IEnumerable<T> IntersectSorted<T>(this IEnumerable<T> a, IEnumerable<T> b) where T : IComparable<T>
    {
        using IEnumerator<T> enumeratorA = a.GetEnumerator();
        using IEnumerator<T> enumeratorB = b.GetEnumerator();

        if (!enumeratorA.MoveNext()) yield break;
        if (!enumeratorB.MoveNext()) yield break;

        while (true)
        {
            int comparison = enumeratorA.Current.CompareTo(enumeratorB.Current);

            if (comparison == 0)
            {
                yield return enumeratorA.Current;
                if (!enumeratorA.MoveNext()) yield break;
                if (!enumeratorB.MoveNext()) yield break;
            }
            else if (comparison < 0)
            {
                if (!enumeratorA.MoveNext()) yield break;
            }
            else
            {
                if (!enumeratorB.MoveNext()) yield break;
            }
        }
    }
}