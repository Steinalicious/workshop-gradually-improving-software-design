# Avoiding Branching and Looping

In this lesson, we will develop a fairly complex feature in a Web application which consists of a significant number of branching and looping instructions.

By the end of the exercise, you will learn how to leverage polymorphism to avoid explicit branching, and such tools as LINQ or Composite pattern to remove looping from the domain-related code.

## Prerequisites

These are the tools and libraries required to run the demos:

  - .NET SDK 7.0 or later - downloaded from [https://dotnet.microsoft.com/en-us/download/dotnet](https://dotnet.microsoft.com/en-us/download/dotnet),
  - An appropriate IDE, such as [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/) (any edition, including Community),
  - SQL Server support - all demos include connection string to a LocalDB database; support for LocalDB is installed as part of [SQL Server Express](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16); any other SQL Server edition would suffice, but with additional requirement that the attendee would need to manage the connection string,
  - [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) or any other tool to easily browse and query the database, including tools and extensions built into the IDE
  - Git client, any

## Preparing the Code

Each lesson is prepared on a separate Git branch. For example, to run the lesson on branching and looping, checkout the branch named `lesson-branching-and-looping`:

```
git checkout lesson-branching-and-looping
```

Any lesson branch contains the starting code of the demonstration. Before running the initial demo, make sure to recreate the database:

```
cd .\Source\
dotnet ef database drop -f --project .\Bookstore\
dotnet ef database update --project .\Bookstore\
```

It is advisable to use `watch run` to keep the application running during the whole development:

```
dotnet watch run --project .\Bookstore\
```