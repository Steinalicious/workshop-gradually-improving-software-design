namespace Bookstore.Domain.Invoices;

public class InvoiceFactory
{
    private DateOnly OnDate { get; }
    private int ToleranceDays { get; }
    private int DelinquencyDays { get; }

    public InvoiceFactory(DateOnly onDate, int toleranceDays, int delinquencyDays) =>
        (OnDate, ToleranceDays, DelinquencyDays) = (onDate, toleranceDays, delinquencyDays);

    public Invoice ToModel(InvoiceRecord representation) =>
        representation.PaymentTime is not null ? new PaidInvoice(representation)
        : representation.DueDate >= OnDate ? new OpenInvoice(representation)
        : representation.DueDate.AddDays(this.ToleranceDays) < OnDate ? new OverdueInvoice(representation)
        : new OutstandingInvoice(representation);

    public IEnumerable<Invoice> ToModels(IEnumerable<InvoiceRecord> representations) =>
        representations.Select(ToModel);
}
