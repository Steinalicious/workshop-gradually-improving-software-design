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