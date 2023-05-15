# Using Specifications and Strategies

In this lesson, we will refine existing features of the Web application by applying specifications to query the database and strategies to transform and process objects.

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

The goal of this exercise is to demonstrate how we can vary behavior through injection of strategies - special polymorphic objects that expose a utility method, the result of which becomes a part of a larger whole. A particular request is to vary formatting of books and their author lists in the `Books.cshtml` and `BookDetails.cshtml` pages. In the `Books.chtml` page, only the authors list should be formatted in different ways, depending on settings. In the `BookDetails.cshtml` page, the "See Also" list of books should be formatted differently, again depending on the application settings.

To manage formatting, add interfaces to the `Domain.Models` namespace that will represent the injectable strategies:

  - `IBibliographicEntryFormatter` - expose a `string Format(Book)` method that will do the formatting; But to be able to format a book, we need to format its authors list, which will be the next interface to define.
  - `IAuthorListFormatter` - expose a similar `string Format(IEnumerable<Person>)` method, so that this becomes another strategy that we can inject every time we must format the list of authors; But formatting the list of authors requires to format a single author, too - hence the third strategy interface, which only deals with separate authors.
  - `IAuthorNameFormatter` - expose a method like `string Format(Person author)` that will do the formatting.

With these interfaces in place, we can start implementing concrete strategies:

  - Create a new namespace `Domain.Models.BibliographicFormatters` that will be home to new classes; each concrete implementation of any of those interfaces above will be one strategy that some class can use at run time.
  - Implement a class named `FullNameFormatter` which implements `IAuthorNameFormatter`; Let this class just append the first and the last name, separated by a single space.
  - Implement a class named `SeparatedAuthorsFormatter` which implements `IAuthorListFormatter`; Let this class receive a separator (default to ", ") and the separator to use before the last author's name (default to " and "), but also the `IAuthorNameFormatter` to format individual authors - this will be the first actual use of a strategy.

This completes the minimum implementation of the two strategies, sufficient to improve the Books.cshtml page:

  - In `Program.cs`, register `FullNameFormatter` as `IAuthorNameFormatter`.
  - In `Program.cs`, register `SeparatedAuthorsFormatter` as `IAuthorListFormatter`; this will work if the constructor had default values for the separators - if not, register a factory function.
  - In `Books.cshtml.cs` inject the `IAuthorListFormatter`
  - Change definition of the `Books` property to become a view model (e.g. a tuple with fields `Guid` Id, authors string and title string).
  - In the `PopulateBooks` utility method, format the result to populate these tuples.
  - In the `Books.cshtml` page, update the UI logic to only render the tuples produced by the backing model.

At this stage, the application is capable of rendering the authors list in the books list using the injectable strategy. The time is to inject a different strategy:

  - Create a new concrete strategy, name it `ShortNameFormatter` and implement the `IAuthorNameFormatter`.
  - Implement the `Format` method to take the first name initial and full last name, making the overall printout shorter.
  - Change `Program.cs` to use `ShortNameFormatter` as `IAuthorNameFormatter` and demonstrate that the `Books.cshtml` page is now producing a different output.

So far, we have successfully applied strategies to one page. We can extend the same process to the `BookDetails.cshtml` page

  - Implement a simple strategy named `TitleOnlyFormatter`, which implements the `IBibliographicEntry` interface and only returns the book's title
  - Add a singleton service to `Program.cs` to map `IBibliographicEntry` to `TitleOnlyFormatter`
  - Change the `BookDetails.cshtml.cs` and `BookDetails.cshtml` to accept `IBibliographicEntry` and use its results.

After this point, we can experiment with more demanding implementations of a strategy:

  - Provide `AcademicNameFormatter` implementation of `IAuthorNameFormatter`, which produces `<last name>, <first name initial>.` format
  - Provide `AcademicAuthorListFormatter` implementation of `IAuthorListFormatter`, which produces up to two author names (in academic format) separated by a comma, or `<first author> et al.` in case of more than two authors.
  - Implement a strategy named `TitlesAndAuthorsFormatter`, which either produces `<authors><separator><title>` or `<title><separator><authors>` format; use a strategy inside this class to select components of the output before separating them with the specified separator; keep in mind that the list of authors might be empty
  - Expose three static factory functions in `TitlesAndAuthorsFormatter` (and hide the constructor), to support three different formats with one class: title then authors, authors then title, academic citation.
  - Register the academic citations factory function in `Program.cs` to format the See Also list in the `BookDetails.cshtml.cs` page.