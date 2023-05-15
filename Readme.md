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

Before proceeding with this lesson, read `IUnitOfWork` and `IRepository` interfaces that were added to the `Data` namespace. Their role is to decouple the rest of the application from the infrastructure. But pages are still deeply dependent on Entity Framework, like `BookDetails.cshtml.cs` in its `OnGet` action handler.

The goal of the first part of this exercise is to introduce specifications, so to decouple the consumers from highly specific data they wish to consume.

  - Define the `ISpecification` interface in the `Data` namespace.
  - Let `ISpecification` expose a method which adds a condition to the specification, like: `ISpecification<TEntity> And(Expression<Func<TEntity, bool>> condition)`
  - Avoid defining a method for the `Or` and `Not` operators.
  - Define two methods which order ascending and descending by the key selector, like: `ISpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector)` and `ISpecification<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector)`; Note that it is not possible to accept `Func<TEntity, TKey>` expressions, because all expressions must be assignable to the same type.
  - Add a method to `IRepository` to query a list of entities, given the specification, like: `Task<List<TEntity>> QueryAsync(ISpecification<TEntity> specification)`.
  - Add a method to `IRepository` to query single or default entity by the specification, like: `Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity> specification)`.

From this point on, we are implementing a concrete specification that will work tightly with the repository applied to the `DbContext`.

  - In the `Data.Implementation` namespace, define the `QueryableSpecification<TEntity>` class that implements `ISpecification<TEntity>`.
  - Let the specification expose a sequence of `Expression<Func<TEntity, bool>>` filters for use in a class that will execute the specification.
  - Let the specification expose a sequence of `(Expression<Func<TEntity, TKey>>, bool)` pairs that will define sorting (bool specifies ascending/descending order).
  - Implement methods of the `QueryableSpecification` class so that it accumulates filters and sorting expressions for further use.
	
The next step is to complete implementation of the `DbSetRepository` class, which must include the new methods defined in the `IRepository` interface.

  - Prepare a private method which applies `ISpecification<TEntity>` to `IQueryable<TEntity>`; Let the method fail if specification is not of the specific implementation type `QueryableSpecification<TEntity>`.
  - Implement `QueryAsync` and `SingleOrDefaultAsync` methods to return the results of applying the specification to the base query of this repository.

This would complete implementation of specifications in the infrastructure layer. Now we can step to creating concrete specifications that would query the database.

  - Create a new namespace `Data.Specifications`. This namespace will contain concrete expressions that can be used to construct a specification.
  - Create a static class `BookSpecs` that will expose a number of static `Expression<Func<Book, bool>>` expressions that can be used in specifications
  - Expose a static method which filters books by `Id`
  - Expose a static method which filters books by the author's initial letter
  - Expose a static method which returns book title (used in sorting)

With these expressions in place, we can substitute all use of LINQ directly in the view pages.

  - In `Program.cs` register `ISpecification` as a service. Ensure that all entity types are supported uniformly: `builder.Services.AddScoped(typeof(ISpecification<>), typeof(QueryableSpecification<>))`.
  - In the `BookDetails.cshtml.cs`, receive `ISpecification<Book>` as a dependency; this specification represents all books (use that when naming the field).
  - Where the book is queried by `Id`, add the filter by `Id` to the injected specification and pass it to the Books repository.
  - To make the call syntax easier to read, modify `BookSpecs` and add extension methods that add filter by `Id` or author initial and an extension method which sorts by title.
  - Repeat the process in the `Books.cshtml.cs` to filter the books by the author's initial; do not forget to use the `OrderByTitle` extension method to sort the books when querying.