namespace Bookstore.Domain.Models.BibliographicFormatters;

public class TitleAndAuthorsFormatter : IBibliographicEntryFormatter
{
    private readonly IAuthorListFormatter _authorListFormatter;
    private readonly string _separator;
    private delegate (string a, string b) JoinStrategy(string title, string authors);
    private readonly JoinStrategy _joinStrategy;

    private TitleAndAuthorsFormatter(IAuthorListFormatter authorListFormatter, JoinStrategy joinStrategy, string separator = ", ") =>
        (_authorListFormatter, _joinStrategy, _separator) = (authorListFormatter, joinStrategy, separator);

    public static IBibliographicEntryFormatter TitleThenAuthors(IAuthorListFormatter authorListFormatter, string separator = ", ") =>
        new TitleAndAuthorsFormatter(authorListFormatter, (t, a) => (t, a), separator);

    public static IBibliographicEntryFormatter AuthorsThenTitle(IAuthorListFormatter authorListFormatter, string separator = ", ") =>
        new TitleAndAuthorsFormatter(authorListFormatter, (t, a) => (a, t), separator);

    public static IBibliographicEntryFormatter Academic() =>
        AuthorsThenTitle(new AcademicAuthorListFormatter(), ", ");

    public string Format(Book book)
    {
        (string a, string b) = _joinStrategy(book.Title, _authorListFormatter.Format(book.Authors));
        if (string.IsNullOrEmpty(a)) return b;
        if (string.IsNullOrEmpty(b)) return a;
        return $"{a}{_separator}{b}";
    }
}
