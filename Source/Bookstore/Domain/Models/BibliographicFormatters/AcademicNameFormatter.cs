namespace Bookstore.Domain.Models.BibliographicFormatters;

public class AcademicNameFormatter : IAuthorNameFormatter
{
    public string Format(Person author) => $"{author.LastName}, {author.FirstName[0]}.";
}