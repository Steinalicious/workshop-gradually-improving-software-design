# Understanding Emergent Objects

In this lesson, we will develop a fairly complex feature in a Web application. To complete the exercise, we will need to design many interconnected classes.

By the end of the exercise, you will learn how to let classes and objects *emerge*, so that they assume their responsibilities most naturally - and pleasantly! - as the design grows more complex.

## Preparing the Code

To run this lesson, checkout the branch named `lesson-emergent-objects`:

```
git checkout lesson-emergent-objects
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

## Task 1 - Implementing a Procedural Solution

In the first part of the exercise, we will try to support book price discounts using procedural coding style and primitive types. Follow these steps to complete the task:

  - Declare a simple type called `DiscountParameters` (record would suffice) that exposes relative discount value (decimal 0 <= x < 1).
  - Register this type as a service in `Program.cs` to create a singleton object with some relative discount (e.g. 0.15); This is the preparatory step before passing the "configured" relative discount to the `BookDetails.cshtml.cs` page.
  - Inject a `DiscountParameters` object into `BookDetails.cshtml.cs`.

This completes the preparatory step - declaring a discount. Now implement the requirements:

- Relative discount value 0 indicates that there is no discount, while a positive value indicates the relative amount of the discount.
   - When there is no discount, only the book's price must appear in the list of `PriceLine`s, with label "Price".
   - When there is the account, then there must be a line with label "Original price", followed by the line indicating amount of the discount, followed by the line with label "TOTAL", indicating the final price with discount deducted from the original price.

With this feature completed, introduce a new possible discount that may interact with the first one, and the rules that may affect all discounts applied to a price:

  - The new discount only applies to a book whose title begins with a preconfigured letter; Add two parameters to the `DiscountParameters` type (the initial letter and the relative discount, e.g. "C" and 0.1).
  - The new discount may apply before or after the first one; Add a parameter to `DiscountParameters` to indicate the order of applications; The second discount in a row applies to the price that remains after the preceding discount has been deducted.
  - Support parallel discounts, i.e. those that all apply to the original price; Add appropriate property to `DiscountParameters`.
  - Support discount cap, i.e. a relative amount that is the maximum to deduct from the original price; Make this another property in `DiscountParameters`.

If you followed through all the requirements, your code must have become very hard to maintain long ago. In the rest of this lesson, we will reimplement all these requirements, but this time applying principles of Object-Oriented Design through the entire process.
