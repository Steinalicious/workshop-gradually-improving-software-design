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

## Task 1 - Implementing the Books Quick Links

The goal of this lesson is to apply the map-reduce principle to implement a new feature in the Bookstore application.

The requirement is to add a strip of buttons or links at the top of the `/books` page, each containing an initial letter (the first letter in the surname) of an author who has published at least one book in the collection. These links would then lead to a list of books filtered by the author's initial letter.

To complete the task, implement these steps:

  - Modify the `/books` page to receive an optional string representing the initial letter of the authors whose books should be displayed
  - Add a collection of strings to the `/books` page, to contain initial letters of authors who have published books in the collection
  - In the `/books` Razor, when the collection of initials is non-empty, display a row of buttons, one button leading to pages of one author; include a special All button to clear the filter
  - Extend the `OnGet` action to filter the books by the author's initial
  - Extend the `OnGet` action to calculate initials of all authors who have published at least one book

## Task 2 - Implementing Book Recommendations

The second exercise in this lesson will mostly be implemented in the `BookDetails` page, where the requirement is to display links to up to three books that share similarities in titles with the book currently displayed in the page.

Similarity is calculated as a number of words shared between the two titles. You can use the `SplitIntoWords` extension method on string (defined in the `Domain.Common.StringExtensions` class) to split any title into individual words.

To complete this task, implement these steps:

  - In the `BookDetails.cshtml.cs` model, expose `IReadOnlyList<Book> RecommendedBooks` with future recommendations
  - Render a table in the `BookDetails.cshtml` Razor page, to contain links from the `RecommendedBooks` property; make sure not to render anything if the collection is empty
  - In the `BookDetails.cshtml.cs` model, Implement a valid LINQ to Entities query that extracts up to three books in descending order of a number of words in common between the two titles; do not attempt to offload the calculation to the database - feel free to load all books from the database for the query to be operational