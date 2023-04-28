using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public class BookLine : InvoiceLine
{
    public Book Book { get; private set; } = null!;

    public BookLine() { }      // Used by EF Core

    public BookLine(Guid id, string label, int quantity, Money price) : base(id, label, quantity, price) { }

    public static BookLine CreateNew(Book book, Money price) =>
        new(Guid.NewGuid(), book.Title, 1, price) { };
}