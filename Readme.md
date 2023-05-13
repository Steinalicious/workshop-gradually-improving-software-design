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

In this lesson, we will develop classes and structs that behave like common values (`int`s, for example). To complete the first part of the lesson, complete implementation of `Currency` and `Money` types:

  - Turn the `Currency` into a `record struct`.
  - Turn the `Money` into a `record struct`; fix `BookDetails.cshtml.cs` to use the nullable value type.
  - Implement all proposed members of the `Money` type, so that it models money amounts with two decimal places.