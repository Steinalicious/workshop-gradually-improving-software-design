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
  - Register this type as a service in `Program.cs` to create a singleton object with relative discount loaded from configuration (see `relativeDiscount` in `Program.cvs`); This is the preparatory step before passing the "configured" relative discount to the `BookDetails.cshtml.cs` page.
  - Inject a `DiscountParameters` object into `BookDetails.cshtml.cs`.

This completes the preparatory step - declaring a discount. Now implement the requirements:

- Relative discount value 0 indicates that there is no discount, while a positive value indicates the relative amount of the discount.
   - When there is no discount, only the book's price must appear in the list of `PriceLine`s, with label "Price".
   - When there *is* the account, then there must be a line with label "Original price", followed by the line indicating amount of the discount, followed by the line with label "TOTAL", indicating the final price with discount deducted from the original price.

With this feature completed, introduce a new possible discount that may interact with the first one, and the rules that may affect all discounts applied to a price:

  - The new discount only applies to a book whose title begins with a preconfigured letter; Add two parameters to the `DiscountParameters` type (the initial letter and the relative discount, e.g. "C" and 0.1).
  - The new discount may apply before or after the first one; Add a parameter to `DiscountParameters` to indicate the order of applications; The second discount in a row applies to the price that remains after the preceding discount has been deducted.
  - Support parallel discounts, i.e. those that all apply to the original price; Add appropriate property to `DiscountParameters`.
  - Support discount cap, i.e. a relative amount that is the maximum to deduct from the original price; Make this another property in `DiscountParameters`.

If you followed through all the requirements, your code must have become very hard to maintain long ago. In the rest of this lesson, we will reimplement all these requirements, but this time applying principles of Object-Oriented Design through the entire process.

## Task 2 - Implementing the Emergent Objects

Remove the parts of the solution developed so far and start over again. The goal is to implement the same requirements, but this time by following the principles of OOD.

  - Create a new namespace `Demo.Discounts`
  - Declare an interface `IDiscount` which will be responsible to modify a price
  - Declare helper record `DiscountApplication` (in the same file where `IDiscount` is declared), exposing string label and `Money` amount
  - Expose a method `DiscountApplication GetDiscountAmount(Money)` which returns the discounted amount.
  - Define a special `NoDiscount` implementation which will be the default implementation when no discount applies - this makes the `IDiscount` definition inflexible enough because it seems like every discount must return the discounted amount.
  - Improve the `IDiscount` interface to: `IEnumerable<DiscountApplication> GetDiscounts(Money)`; This definition allows for no discount (returning an empty sequence), or even a discount which produces more than one change in the specification.
  - Implement `NoDiscount` to return an empty sequence of `DiscountApplication`s.
  - Register `NoDiscount` implementation of `IDiscount` in `Program.cs`.
  - Inject `IDiscount` into the `BookDetails.cshtml.cs` model, but don't use it yet.

This completes the injecting part and now the page model finally has an *object* to work with. In the next stage, we will implement a flat-rate relative discount.

  - In the `Domain.Discounts.Implementation` namespace, implement `RelativeDiscount` variant of `IDiscount`.
  - In `Program.cs`, use the `relativeDiscount` value loaded from configuration to register `RelativeDiscount` in place of `NoDiscount`.
  - In `BookDetails.cshtml.cs` call the `IDiscount.GetDiscountAmounts()`, analyze the sequence returned and populate the `PriceSpecification` list accordingly.
  - Define a new kind of a discount named `TitlePrefixDiscount` in `Domain.Discounts.Implementation`, which applies if the book's title begins with a specified prefix.
  - In `Program.cs`, register `TitlePrefixDiscount` with 15% discount off of books starting with "C" - this will help in development testing.
  - In `BookDetails.cshtml.cs`, we cannot resolve a discount which depends on a book; Introduce a helper type `Domain.Discounts.DiscountContext` which wraps the context within which the discount is applied - only add a nullable `Book` at first.
  - Expose a new method on the `IDiscount` interface: `IDiscount Within(DiscountContext context)` and implement it in all concrete discounts that exist.
  - In the `BookDetails.cshtml.cs`, use the `Book` model to constrain the `IDiscount` injected into the page just before calculating effective discounts.

This completes the part of development where we have successfully supported individual discounts. We need to support multiple discounts operating as a whole now:

  - In the `Domain.Discounts.Implementation` namespace, implement special class `ChainedDiscounts`; This discount groups multiple other discounts, applies them in order and always supplies a contained discount with the money amount that remained after the preceding discounts were applied; Make sure to stop propagation when the total input amount was exhausted.
  - Implement special class `ParallelDiscounts` which operates the same as `ChainedDiscounts`, with a difference that all contained discounts observe the same input price to which they apply.
  - Implement special class `DiscountCap` which caps the total discounted amount by a discount that it contains.
  - In the `Domain.Discounts`, implement a helper static class with extension methods `Then`, `And` and `CapTo`, which wrap a discount into one of the three variants we just created.
  - In the `Program.cs`, configure the discounts to demonstrate use of these new classes.
