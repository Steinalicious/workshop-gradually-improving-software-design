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

## Tasks

Most of the relevant features for this lesson are located in the `/invoices` page. The application is capable of customizing the display style of invoices based on their status, which is calculated polymorphically.

As a preparation for the upcoming tasks, the `InvoiceFactory` class also contains the `DelinquencyDays` parameter which is loaded from the JSON configuration file (`appsettings.Development.json`). That is the number of days past `DueDate` after which the invoice becomes delinquent and the `Customer` to which it was issued should be notified of one or more missed payments.

From this point on, the task is to display a table at the bottom of the `/invoices` page which displays one row per delinquent `Customer`, showing the total unpaid dues by that customer.

## Step-by-Step Tasks

To complete this lesson, perform the following tasks:

  - Define a `DueNotification` record which represents total dues (`Money`) by a customer (`Customer` object)
  - Implement an expression which filters out all the invoices except those that are more than `DelinquencyDays` (config parameter) past `DueDate`
  - Display a table in the `/invoices` page which lists dues by delinquent customers; use LINQ to populate the table
  - Imagine that construction of total dues record is much more complex; introduce `IDelinquent` interface which returns a sequence of `DueNotification` record instances
  - Implement immutable composite `IDelinquent`s: one grouping invoices from one customer, and one grouping invoices by many customers
  - Implement factory methods that can construct `IDelinquent` instances fluently as more invoices are added to the structure
  - Implement an empty `IDelinquent` which represents the starting object in the process of building a larger structure
