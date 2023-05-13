namespace Bookstore.Domain.Models.BibliographicFormatters;

public class TitleOnlyFormatter : IBibliographicEntryFormatter
{
    public string Format(Book book) => book.Title;
}