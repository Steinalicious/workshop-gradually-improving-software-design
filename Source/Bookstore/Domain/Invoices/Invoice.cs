using System.Diagnostics;
using System.Linq.Expressions;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public abstract class Invoice
{
    protected InvoiceRecord Representation { get; }

    protected Invoice(InvoiceRecord representation) => Representation = representation;

    public InvoiceLine Add(Book book, Money price)
    {
        if (this.Representation.Lines.OfType<BookLine>().FirstOrDefault(line => line.Book.Id == book.Id) is BookLine line)
        {
            line.Increment(1, price);
            return line;
        }

        InvoiceLine newLine = BookLine.CreateNew(this.Representation, book, price);
        this.Representation.Lines.Add(newLine);
        return newLine;
    }
}

public class PaidInvoice : Invoice
{
    internal PaidInvoice(InvoiceRecord representation) : base(representation) { }

    public DateTime PaymentTime => base.Representation.PaymentTime.HasValue
        ? this.Representation.PaymentTime.Value
        : throw new UnreachableException();
}

public abstract class UnpaidInvoice : Invoice
{
    protected UnpaidInvoice(InvoiceRecord representation) : base(representation) { }

    public Invoice Pay(DateTime at)
    {
        base.Representation.PaymentTime = at;
        return new PaidInvoice(base.Representation);
    }
}

public class OpenInvoice : UnpaidInvoice
{
    internal OpenInvoice(InvoiceRecord representation) : base(representation) { }
}

public class OutstandingInvoice : UnpaidInvoice
{
    internal OutstandingInvoice(InvoiceRecord representation) : base(representation) { }
}

public class OverdueInvoice : UnpaidInvoice
{
    internal OverdueInvoice(InvoiceRecord representation) : base(representation) { }
}
