namespace Bookstore.Domain.Models;

public interface IAuthorListFormatter
{
    string Format(IEnumerable<Person> authors);
}