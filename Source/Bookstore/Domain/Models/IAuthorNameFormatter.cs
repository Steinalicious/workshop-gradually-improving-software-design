namespace Bookstore.Domain.Models;

public interface IAuthorNameFormatter
{
    string Format(Person author);
}