namespace Bookstore.Domain.Models.BibliographicFormatters;

public class ShortNameFormatter : IAuthorNameFormatter
{
    public string Format(Person author) => $"{author.FirstName[0]}. {author.LastName}";
}
