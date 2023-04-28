using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.Empty;
    public Guid CustomerId { get; private set; } = Guid.Empty;
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    internal ICollection<InvoiceLine> Lines { get; } = new List<InvoiceLine>();

    private Invoice() { }      // Used by EF Core

    public static Invoice CreateNew(Guid customerId) =>
        new()
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
        };

    public void Add(Book book, Money price)
    {
        if (this.Lines.OfType<BookLine>().FirstOrDefault(line => line.Book.Id == book.Id) is BookLine line)
        {
            line.Increment(1, price);
        }
        else
        {
            this.Lines.Add(BookLine.CreateNew(book, price));
        }
    }
}
