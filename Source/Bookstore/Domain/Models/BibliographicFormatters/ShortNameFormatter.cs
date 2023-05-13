namespace Bookstore.Domain.Models.BibliographicFormatters;

public class ShortNameFormatter : IAuthorNameFormatter
{
    public string Format(Person author) => $"{author.FirstName.Substring(0, 1)}. {author.LastName}";
}
