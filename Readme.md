# Modeling Optional Objects

In this lesson, we will design a monad which represents an object that may or may not exist. We will use optional objects to simplify the domain models, and then combine them with TPL to model awaitable optional objects that are useful when working with infrastructure and UI.

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

In this lesson, we shall manage objects that can be missing. For instance, in `Data.IRepository<TEntity>`, the result of the `SingleOrDefaultAsync` is an object that might legally be missing. Missing objects are also common in domain modeling. `Domain.Models.Citation` may contain elements that point to an author or a book.

  - Introduce a class `CitationExtensions` that will host additional behavior on `Citation`.
  - Define a method `IEnumerable<Guid> GetAuthorIds()` which returns zero or more `Id`s of authors pointed to by the citation.
  - Define a method `Guid? TryGetBookId()` which returns an `Id` of a book quoted in the citation or `null` if there is none.
  - Define a method `bool HasNoAuthors()` which returns `true` if there are no author references in the citation.
  - Define a special method `Guid? TryGetBookIdNoAuthors()` which does the same as `TryGetBookId`, but returns `null` if there are any authors in the citation; These three methods will help design adaptable UI, which may render the entire `Citation` as an anchor tag pointing to a book, or as a sequence of separate HTML elements, some of which could be anchors linking to authors, another a link to a book.

Dealing with missing objects may complicate the consuming code. Each caller must include both the positive and the negative branch, which quickly goes out of hand when there are multiple steps, each having positive and negative outcome.

In the rest of the exercise, we will wrap this logic into a monad, referred to as `Maybe` or `Option`.

  - In the `Common` namespace (not `Domain.Common`!), add a generic class named `Option<T>`.
  - For performance reasons, turn this into a `record struct`.
  - Wrap a nullable reference and constrain the class to reference types `T`.
  - Implement `Option<TResult> Map(Func<T, TResult>)` and `Option<TResult> MapOptional(Func<T, Option<TResult>>)` methods that are optionally transforming the content.
  - Implement `T Reduce(T)` and `T Reduce(Func<T>)` methods that are extracting the content.
  - Implement implicit operator to `T?` to extract nullable content, for pattern matching expressions.
  - Implement the `Option<T> Audit(Action<T> audit)` method which applies an action to content, if it exists.
  - Expose static factory methods `Option<T> Some(T)` and `Option<T> None()`; the required public parameterless constructor is equivalent to `None`; a proper constructor `Option(T?)` should be private.

This completes implementation of a monad which can substitute nullable logic at the calling site. However, we cannot apply this class to the `CitationExtensions.TryGetBookId()` and `CitationExtensions.TryGetBookIdNoAuthors()`, because the methods are returning `Guid`, which is a value type. We need a similar monad that would support value types:

  - In the `Common` namespace, define the `ValueOption<T>` `record struct` where `T` is a `struct`
  - Implement methods equivalent to those in the `Option`: static factory methods, `Map`, `MapOptional`, `Audit`, `Reduce`.
  - Create a bridge to class `Option` types, a pair of methods `Option<TResult> MapToObject(Func<T, TResult>) where TResult : class` and `Option<TResult> MapOptionalToObject(Func<T, Option<TResult>) where TResult : class`
  - Do the same in the `Option<T>` type, exposing corresponding `MapToValue` and `MapOptionalToValue` methods.
  - Define a static class `OptionalExtensions` which defines extension methods `Option<T> AsOption<T>(T?) where T : class` and `ValueOption<T> AsOption<T>(T?) where T : struct`.

Now we are ready to apply optional objects to `CitationExtensions`:

  - Change implementation to include a filtering method `Option<Citation> WithNoAuthors()`
  - Change implementation to return `ValueOption`s from `TryGetBookId` and `TryGetBookIdNoAuthors`
  - Introduce `Common.OptionalLinq` static class that exposes an extension methods `FirstOrNone` and `SingleOrNone`

This will help implement optional logic that depends on the content of a `Citation`. However, actual operations are asynchronous, like in `IRepository<TEntity>` or the `OnGet` method in `BookDetails.cshtml.cs`. We need to apply another cross-cutting concern, asynchronous execution, to optional objects to make them ubiquitous.

The `Task` class is already a monad. It is backed by `async` and `await` keywords in C# for simplicity, but its `ContinueWith` is by all means equivalent to the `Option.Map` method.

We will now combine the powers of the `Task` monad and the `Option` monad to build parallel options:

  - In the `Common` namespace, define the static `AsyncOptionExtensions` class
  - Implement method `async Task<Option<TResult>> MapAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> map)`
  - Implement method `async Task<Option<TResult>> MapToObjectAsync<T, TResult>(this ValueOption<T> option, Func<T, Task<TResult>> map)`
  - Implement method `async Task<Option<T>> AuditAsync<T>(this Option<T> option, Func<T, Task> auditAsync) where T : class`

These methods will allow applying asynchronous mapping functions to a common option, turning the overall result into an awaitable object which is subject to use of `await` keyword in an `async` containing method.

Now we can extend `Task<Option<T>>` with convenient awaitable or common functions, so that the result again becomes an awaitable object, either optional or not:

  - Implement method `async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, TResult> map)`
  - Implement method `async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, Task<TResult>> map)`
  - Implement method `async Task<Option<T>> AuditAsync<T>(this Task<Option<T>> option, Func<T, Task> auditAsync)`
  - Implement method `async Task<Option<TResult>> Map<T, TResult>(this Task<Option<T>> option, Func<T, TResult> map)`
  - Implement method `async Task<T> Reduce<T>(this Task<Option<T>> option, T whenNone)`
  - Implement method `async Task<T> Reduce<T>(this Task<Option<T>> option, Func<T> whenNone)`
  - Implement method `async Task<T> ReduceAsync<T>(this Task<Option<T>> option, Func<Task<T>> whenNone)`

We can now finally start consuming the optional objects. We will reimplement the `OnGet` action in `BookDetails.cshtml.cs`. We will also apply partial views that can adapt to the content of a `Citation` and render it in the best way.

Open the `Shared._CitationPartial.cshtml` view and observe how it is rendering one partial view or another depending on whether the `Citation` as a whole is a book citation, or it contains fine-grained references like author and book references.