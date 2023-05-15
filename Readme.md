# Applying the Map-Reduce Principle

In this lesson, we will continue developing the Web application, this time putting stress on sequences of objects and the ways we can process them.

## Preparing the Code

To run this lesson, checkout the branch named `lesson-branching-and-looping`:

```
git checkout lesson-branching-and-looping
```

This branch contains the starting code of the demonstration of emergent objects. Before running the initial demo, make sure to recreate the database:

```
cd .\Source\
dotnet ef database drop -f --project .\Bookstore\
dotnet ef database update --project .\Bookstore\
```

It is advisable to use `watch run` to keep the application running during the whole development:

```
dotnet watch run --project .\Bookstore\
```

## Step-by-Step Tasks

The goal of this lesson is to demonstrate the power of working with sequences. That includes intensive use of LINQ, but also defining custom LINQ extensions that can relieve the consuming code from having to implement complex transforms using other means (most notably: procedural constructs).

To complete this lesson, perform these tasks in order:

  - Implement an extension to `IEnumerable<T>` which prints its content in a grid: `void PrintGrid<T>(this IEnumerable<T> values, int columnWidth, int columnsCount)`; use this extension method to print the values produced by sequences in this demo; optionally, implement a convenient `PrintGrid` that operates on `IEnumerable<int>` to include right-alignment and thousands separator
  - Define a method called `GetPrimeNumbers` that returns a sequence of `int`s that are prime; return an empty sequence at first; this can be a static method, so that the methods it will use can be defined as extension methods
  - Define a method that returns prime candidates: 2, 3, and all valid `int`s in form `6k+1` or `6k-1`
  - Define a method which returns divisors of a number, given the list of divisor candidates; this method will form the core of the `GetPrimeNumbers` method
  - Implement method `bool IsPrime(int)` which combines `GetPrimeCandidates` and `GetDivisors` to determine if a given number is prime
  - Implement an extension method `IntersectSorted` which intersects two sorted sequences of comparable items: `IEnumerable<T> IntersectSorted<T>(this IEnumerable<T> a, IEnumerable<T> b) where T : IComparable<T>`
  - Implement method `OneLessThanTwosPower`, which returns all valid `int`s in form `2^n-1`; beware of the arithmetic overflow on `int.MaxValue`
  - Combine `OneLessThanTwosPower`, `GetPrimeNumbers` and `IntersectSorted` operator to calculate Mersenne primes smaller than 1,000,000