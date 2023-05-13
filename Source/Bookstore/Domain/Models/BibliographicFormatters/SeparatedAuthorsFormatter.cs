namespace Bookstore.Domain.Models.BibliographicFormatters;

public class SeparatedAuthorsFormatter : IAuthorListFormatter
{
    private readonly string _separator;
    private readonly string _lastSeparator;
    private readonly IAuthorNameFormatter _authorNameFormatter;

    public SeparatedAuthorsFormatter(IAuthorNameFormatter singleAuthorFormatter, string separator = ", ", string lastSeparator = " and ") =>
        (_authorNameFormatter, _separator, _lastSeparator) = (singleAuthorFormatter, separator, lastSeparator);

    public string Format(IEnumerable<Person> authors) =>
        _separator == _lastSeparator ? FormatUniform(authors) : FormatWithLast(authors.ToArray());

    private string FormatUniform(IEnumerable<Person> authors) =>
        string.Join(_separator, authors.Select(_authorNameFormatter.Format));

    private string FormatWithLast(Person[] authors)
    {
        if (authors.Length < 2) return FormatUniform(authors);
        var lastAuthor = authors[^1];
        IEnumerable<Person> otherAuthors = authors[..^1];
        return FormatUniform(otherAuthors) + _lastSeparator + _authorNameFormatter.Format(lastAuthor);
    }
}