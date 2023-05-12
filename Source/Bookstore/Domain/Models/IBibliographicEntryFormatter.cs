namespace Bookstore.Domain.Models;

public interface IBibliographicEntryFormatter
{
    string Format(Book book);
}