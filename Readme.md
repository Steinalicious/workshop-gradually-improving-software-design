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

## Task 1 - Implementing the branching logic

In this lesson, we will implement the invoice statuses, managed by the `Domain.Models.Invoice` and related classes. An invoice can assume one of these statuses:

  - Paid - when its `PaymentTime` has a value,
  - Open - when its `PaymentTime` has no value and its `DueDate` has not passed,
  - Outstanding - has no `PaymentTime`, `DueDate` has passed, but number of days since `DueDate` is not greater than a specific `ToleranceDays` parameter, or
  - Overdue - same as Outstanding, with number of days past `DueDate` being greater than `ToleranceDays`.

The `ToleranceDays` parameter is specified as `Invoicing:ToleranceDays` config parameter (provided in the `appsettings.Development.json`). It is safe to take current time or date of the Web request, and to use that time to compare with `DueDate`.

To complete the first stage of the lecture, implement the `Invoices` page to satisfy these requirements:

  - Display values in the `Status` column. In each row, the value should be one of: Paid on (date); Due by (date); Outstanding since (date) or Overdue since (date).
  - Color the rows of the table depending on the status (suggestion: choose green, amber, pale red and red color for Paid, Open, Outstanding and Overdue, respectively).

## Task 2 - Implementing the polymorphic solution

One intrinsic problem with previous request is that, in addition to depending on the state of the row in the database, the result also depends on the time when it is executed. It is not possible to save the invoice state to the database, because state depends on the time when it is calculated.

To complete the second stage of the lecture, ensure that the solution satisfies these requirements:

  - There are separate classes modeling four possible states of the invoice: `PaidInvoice`, `OpenInvoice`, `OutstandingInvoice` and `OverdueInvoice`.
  - There is the `InvoiceFactory` which converts the database record into an `Invoice` subclass instance; the factory can encapsulate any information it needs, such as request time or tolerance days parameter.
  - Reimplement features from the previous task, using polymorphic behavior of the `Invoice` class hierarchy.
  - Add the column to the right of all the data in the table, and place a button with label "Mark as paid" in every row that displays an unpaid invoice (in any state). When this button is pressed, set the invoice's payment time to current time, update it in the database and refresh the table.

Since invoices may be updated during the request execution, and any changes must be tracked by the `DbContext`, one possible solution is to let each `Invoice` object wrap the underlying database record.

## Task 3 - Implementing the looping classes

Start by checking out the `lesson-branching-and-looping-stage1` branch and updating the database schema:

```
git checkout lesson-branching-and-looping-stage1

dotnet ef database update --project .\Bookstore\
```
