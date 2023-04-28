using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.Empty;
    public Guid CustomerId { get; private set; } = Guid.Empty;
    public DateTime IssueTime { get; private set; }
    public DateOnly DueDate { get; private set; }
    public DateTime? PaymentTime { get; private set; }
    internal ICollection<InvoiceLine> Lines { get; } = new List<InvoiceLine>();

    private Invoice() { }      // Used by EF Core

    public static Invoice CreateNew(Guid customerId, DateTime IssueTime, DateOnly dueDate) =>
        new()
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            IssueTime = IssueTime,
            DueDate = dueDate
        };

    public static Invoice CreateNew(Guid customerId, DateTime IssueTime, int dueAfterDays) =>
        CreateNew(customerId, IssueTime, DateOnly.FromDateTime(IssueTime).AddDays(dueAfterDays));

    public void Pay(DateTime at)
    {
        if (this.PaymentTime is not null) throw new InvalidOperationException("Invoice already paid");
        this.PaymentTime = at;
    }

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
