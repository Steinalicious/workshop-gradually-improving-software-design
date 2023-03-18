namespace Bookstore.Models;

public class Book
{
    public int Id { get; private set; } = 0;
    public string Title { get; private set; } = string.Empty;
    private ICollection<Person> AuthorsCollection { get; set; } = new List<Person>();
    public IEnumerable<Person> Authors => AuthorsCollection;

    private Book() { }  // Used by EF Core

    public static Book CreateNew(string title, params Person[] authors) => new()
    {
        Title = title,
        AuthorsCollection = authors.ToList()
    };
}
