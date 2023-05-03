using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Invoices;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class InvoicesModel : PageModel
{
    public record InvoiceRow(Guid Id, int Ordinal, string Label, string IssueDate, string Status, Money Total, string Style, bool AllowPayment);

    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<InvoiceRecord> _invoicesSeed;
    private readonly InvoiceFactory _invoiceFactory;

    public IEnumerable<InvoiceRow> Invoices { get; private set; } = Enumerable.Empty<InvoiceRow>();

    public InvoicesModel(ILogger<IndexModel> logger, BookstoreDbContext context, IDataSeed<InvoiceRecord> invoicesSeed, InvoiceFactory invoiceFactory) => 
        (_logger, _context, _invoicesSeed, _invoiceFactory) = (logger, context, invoicesSeed, invoiceFactory);

    public async Task OnGet()
    {
        await _invoicesSeed.SeedAsync();
        await PopulateInvoices();
    }

    public async Task<IActionResult> OnPost(Guid invoiceId)
    {
        _logger.LogInformation($"Payment for invoice {invoiceId} received.");
        await PopulateInvoices();
        return Page();
    }

    private async Task PopulateInvoices()
    {

        var records = await _context.Invoices
            .Include(invoice => invoice.Customer)
            .Include(invoice => invoice.Lines)
            .OrderBy(invoice => invoice.IssueTime)
            .ToListAsync();
        var invoices = _invoiceFactory.ToModels(records);

        this.Invoices = invoices.Select((record, index) => ToRow(index + 1, record)).ToList();
    }

    private static InvoiceRow ToRow(int ordinal, Invoice invoice) =>
        new InvoiceRow(
            invoice.Id, ordinal, invoice.Label,
            invoice.IssueDate.ToString("MM/dd/yyyy"),
            $"{invoice.Status.prefix} {invoice.Status.date}",
            invoice.Total, ToStyle(invoice), invoice is UnpaidInvoice);

    private static string ToStyle(Invoice invoice) =>
        invoice.GetType().Name.Replace("Invoice", "").ToLower();
}
