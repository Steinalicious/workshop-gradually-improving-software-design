namespace Bookstore.Books.Models;

public class Book
{
    public int Id { get; private set; } = 0;
    public string Title { get; private set; } = string.Empty;
    private ICollection<BookAuthor> AuthorsCollection { get; set; } = new List<BookAuthor>();
    public IEnumerable<Person> Authors => AuthorsCollection.Select(author => author.Person);

    private Book() { }  // Used by EF Core

    public static Book CreateNew(string title, params Person[] authors)
    {
        Book book = new() { Title = title };
        book.AuthorsCollection = BookAuthor.CreateMany(book, authors).ToList();
        return book;
    }
}
