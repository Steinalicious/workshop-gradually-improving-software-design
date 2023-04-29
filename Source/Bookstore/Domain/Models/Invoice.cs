using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.Empty;
    public Customer Customer { get; private set; } = null!;
    public DateTime IssueTime { get; private set; }
    public DateOnly DueDate { get; private set; }
    public DateTime? PaymentTime { get; private set; }
    internal List<InvoiceLine> Lines { get; } = new List<InvoiceLine>();

    public Money TotalAmount =>
        this.Lines.Aggregate(Money.Zero, (total, line) => total + line.Price);

    private Invoice() { }      // Used by EF Core

    public static Invoice CreateNew(Customer customer, DateTime IssueTime, DateOnly dueDate) =>
        new()
        {
            Id = Guid.NewGuid(),
            Customer = customer,
            IssueTime = IssueTime,
            DueDate = dueDate
        };

    public static Invoice CreateNew(Customer customer, DateTime IssueTime, int dueAfterDays) =>
        CreateNew(customer, IssueTime, DateOnly.FromDateTime(IssueTime).AddDays(dueAfterDays));

    public void Pay(DateTime at)
    {
        if (this.PaymentTime is not null) throw new InvalidOperationException("Invoice already paid");
        this.PaymentTime = at;
    }

    public InvoiceLine Add(Book book, Money price)
    {
        if (this.Lines.OfType<BookLine>().FirstOrDefault(line => line.Book.Id == book.Id) is BookLine line)
        {
            line.Increment(1, price);
            return line;
        }

        InvoiceLine newLine = BookLine.CreateNew(this, book, price);
        this.Lines.Add(newLine);
        return newLine;
    }
}
