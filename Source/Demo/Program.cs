using System.Linq;
using Demo;

// Primes.GetPrimeNumbers().Take(10_000).PrintGrid(8, 10);
// OneLessThanTwosPower().PrintGrid(20, 5);

var mersennePrimes = Primes.GetPrimeNumbers()
    .IntersectSorted(OneLessThanTwosPower().TakeWhile(x => x < 1_000_000));
mersennePrimes.PrintGrid(20, 5);

IEnumerable<int> OneLessThanTwosPower() => Enumerable.Range(0, 30)
    .Select(x => (1 << x) - 1)
    .Concat(new[] { int.MaxValue });