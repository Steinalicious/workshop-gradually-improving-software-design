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

In this exercise, we will utilize optional objects to develop rich user experience with book citations.

  - In `Books.cshtml.cs`, define the `Books` property to contain `Citation` objects representing both authors and the title of each book.
  - In the `PopulateBooks` method, transform the authors list and the book into `Citation`s to populate the model property.
  - In the `Books.cshtml` view, use partial view `_CitationPartial` in two places, to render authors and the book's title; This view is already fully operational, and the results will become immediately visible after htis change.
  - In the `BookDetails.cshtml.cs`, declare the `RecommendedBooks` as a list of `Citation` objects and update the `PopulateRecommendedBooks` method accordingly.
  - In the `BookDetails.cshtml`, use the partial view `_CitationPartial` to render the recommended books.
