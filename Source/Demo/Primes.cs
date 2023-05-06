static class Primes
{
    public static IEnumerable<int> GetPrimeNumbers() => GetPrimeCandidates().Where(IsPrime);

    private static bool IsPrime(this int number) =>
        !GetDivisors(number, GetPrimeCandidates().LessOrEqualTo((int)Math.Sqrt(number))).Any();

    private static IEnumerable<int> GetDivisors(this int number, IEnumerable<int> candidates) =>
        candidates.Where(candidate => number % candidate == 0);

    private static IEnumerable<int> LessOrEqualTo(this IEnumerable<int> candidates, int limit) =>
        candidates.TakeWhile(candidate => candidate <= limit);

    private static IEnumerable<int> GetPrimeCandidates()
    {
        yield return 2;
        yield return 3;
        yield return 5;
        int last = 5;
        int step = 2;
        while (int.MaxValue - step >= last)
        {
            last += step;
            step = 6 - step;
            yield return last;
        }
    }
}
