using Bookstore.Common;

namespace Bookstore.Domain.Models;

public static class CitationExtensions
{
    public static IEnumerable<Guid> GetAuthorIds(this Citation citation) =>
        citation.OfType<BookAuthorSegment>().Select(s => s.AuthorId);

    public static Option<Citation> WithNoAuthors(this Citation citation) =>
        citation.ContainsAuthors() ? Option<Citation>.None() : Option<Citation>.Some(citation);

    private static bool ContainsAuthors(this Citation citation) =>
        citation.Any(s => s is BookAuthorSegment);

    public static ValueOption<Guid> TryGetBookId(this Citation citation) =>
        citation.OfType<BookTitleSegment>().Select(title => title.BookId).SingleOrNone();

    public static ValueOption<Guid> TryGetBookIdNoAuthors(this Citation citation) =>
        citation.WithNoAuthors().MapOptionalToValue(citation => citation.TryGetBookId());
}