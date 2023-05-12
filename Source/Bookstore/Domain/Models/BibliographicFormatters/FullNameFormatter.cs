using System.Text;

namespace Bookstore.Domain.Models.BibliographicFormatters;

public class FullNameFormatter : IAuthorNameFormatter
{
    public string Format(Person author) => $"{author.FirstName} {author.LastName}";
}