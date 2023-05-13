# Avoiding Branching and Looping

In this lesson, we will develop a fairly complex feature in a Web application which consists of a significant number of branching and looping instructions.

By the end of the exercise, you will learn how to leverage polymorphism to avoid explicit branching, and such tools as LINQ or Composite pattern to remove looping from the domain-related code.

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

In this part of the lesson, we will develop a rich model that supports HTML formatting that we can vary by configuration. It will be the formatting of book citations again, this time adding styles and clickable links.

  - Define a new abstract record `CitationSegment` that represents a part of the bibliographic entry, such as the author's name or the book's title; The `CitationSegment` exposes a `string` representation, which is used in rendering.
  - Define three derived `sealed record`s that represent book author (including author's Id for further reference), book title (including the book's Id) and a plain text which is not representing any higher object.
  - Augment the `IAuthorNameFormatter` to expose `IEnumerable<CitationSegment> FormatToSegments(Person)` method.
  - Provide default implementation of `IAuthorNameFormatter Format(Person)` method to concatenate text representations of all segments.
  - Implement the `FormatToSegments` method in the `FullNameFormatter`; Consider removing the old `Format` method (not necessary, and maybe not even advisable in this case).
  - Implement the `FormatToSegments` method in the `AcademicNameFormatter` class.
  - Implement the `FormatToSegments` method in the `ShortNameFormatter` class.
  
After these steps, the application should continue working the same as before. All strings produced by formatters will remain the same as they were, and the views will not use the new `FormatToSegments` method, nor benefit from its rich results.

The next stage in development is to understand the benefits from immutable classes. Instead of returning a sequence of `CitationSegment` objects, we can design a special class that wraps multiple `CitationSegment`s and exposes useful operations to join and string them together. Follow the steps below to implement this class.

  - Define a `Citation` class, which will be an immutable collection of `CitationSegment` objects.
  - Let the `Citation` store `CitationSegment` instances in an immutable list; Implement appropriate constructors and the `Empty` static factory property getter.
  - Implement the immutable `Add` method that utilizes this immutable list.
  - Return `Citation` from `IAuthorNameFormatter` and its concrete implementations; Rename the method to `Citation ToCitation(Person)` for better readability.
  - The next step is to make `Citation` look similar to `CitationSegment`, up to the point of making the two exchangeable. Expose a public property `string Text` on the `Citation` class and concatenate `Text` values of the underlying `CitationSegment` objects.
  - Use the `Citation.Text` in the `IAuthorNameFormatter.Format(Person)` default implementation. Observe the simplifying effect on `IAuthorNameFormatter` as we started developing the new class. If we can keep `Format` method at the level of default implementation, then we will be able to retire it after all its callers are gone.
  - The next step is to improve construction of `Citation`s from a single `CitationSegment`. On the `Citation` class, expose an implicit conversion operator from `CitationSegment` to `Citation`: `public static implicit operator Citation(CitationSegment segment)`. On the `CitationSegment` abstract record, expose an implicit static operator from string, which creates a `TextSegment`. This will allow passing a plain string to the `Citation.Add` method.
  - Return a new `CitationSegment` object from all concrete implementations of `IAuthorNameFormatter`. Observe how all classes are becoming significantly simpler this way.
  - Augment the `IAuthorListFormatter` interface to expose equivalent `Citation Format(IEnumerable<Person> authors)` method; provide the default implementation of the existing `Format` method.
  - Before implementing `IAuthorListFormatter` concrete classes, add `params CitationSegment[] other` to the `Citation.Add` method to make it useful to add multiple `CitationSegment` objects at once.
  - Define `Add` method's overload which receives another `Citation` and adds it to the current `Citation` object.
  - Define static 'Citation.Join` method which joins multiple `Citation` objects separated by the specified `Citation` object.
  - Complete implementation of the `AcademicAuthorListFormatter` concrete `IAuthorListFormatter`. Use array pattern matching to complete the class.
  - Repeat exercise on the `IBibliographicEntryFormatter` interface and concrete classes.
  - Switch all consumers to use `ToCitation` methods of `IAuthorListFormatter` and `IBibliographicEntryFormatter`; Remove the old `Format` methods with default implementations.

The final step is to use this new object model to improve the UI:
  - Let the `Citation` class implement `IEnumerable<CitationSegment>` and use the contained immutable list to return the enumerator.
  - In the `Book.cshtml.cs` make the `Books` property contain `Citation`s instead of strings; use the new content to render specific segments as `span`s with styles from `books.css`.
  - Repeat exercise on the See Also list in `BookDetails.cshtml.cs` and `BookDetails.cshtml`, also using `books.css` to ensure that each segment is styled correctly.
  