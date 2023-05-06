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

The goal of this lesson is to apply the map-reduce principle to implement a new feature in the Bookstore application. The requirement is to add a strip of buttons or links at the top of the `/books` page, each containing an initial letter (the first letter in the surname) of an author who has published at least one book in the collection. These links would then lead to a list of books filtered by the author's initial letter.

To complete the task, implement these steps:
  - Modify the `/books` page to receive an optional string representing the initial letter of the authors whose books should be displayed
  - Add a collection of strings to the `/books` page, to contain initial letters of authors who have published books in the collection
  - In the `/books` Razor, when the collection of initials is non-empty, display a row of buttons, one button leading to pages of one author; include a special All button to clear the filter
  - Extend the `OnGet` action to filter the books by the author's initial
  - Extend the `OnGet` action to calculate initials of all authors who have published at least one book