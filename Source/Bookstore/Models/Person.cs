namespace Bookstore.Models;

public class Person
{
    public int Id { get; private set; } = 0;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private Person() { }  // Used by EF Core

    public static Person CreateNew(string firstName, string lastName) => new()
    {
        FirstName = firstName,
        LastName = lastName
    };

    public static Person CreateNew(string firstName) => new()
    {
        FirstName = firstName
    };
}