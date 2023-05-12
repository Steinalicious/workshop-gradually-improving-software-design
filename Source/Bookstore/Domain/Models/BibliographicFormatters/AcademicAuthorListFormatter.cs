namespace Bookstore.Domain.Models.BibliographicFormatters;

public class AcademicAuthorListFormatter : IAuthorListFormatter
{
    public string Format(IEnumerable<Person> authors) =>
        Format(authors.ToArray());

    private string Format(Person[] authors)
    {
        IAuthorNameFormatter nameFormatter = new AcademicNameFormatter();
        if (authors.Length < 3) return string.Join(", ", authors.Select(nameFormatter.Format));
        return nameFormatter.Format(authors[0]) + " et al.";
    }
}