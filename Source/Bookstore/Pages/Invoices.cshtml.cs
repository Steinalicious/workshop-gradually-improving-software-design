using Bookstore.Data.Seeding;
using Bookstore.Domain.Invoices;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class InvoicesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<InvoiceRecord> _invoicesSeed;

    public IEnumerable<InvoiceRecord> Invoices { get; private set; } = Enumerable.Empty<InvoiceRecord>();

    public InvoicesModel(ILogger<IndexModel> logger, BookstoreDbContext context, IDataSeed<InvoiceRecord> invoicesSeed) => 
        (_logger, _context, _invoicesSeed) = (logger, context, invoicesSeed);

    public async Task OnGet()
    {
        await _invoicesSeed.SeedAsync();

        this.Invoices = await _context.Invoices.Include(invoice => invoice.Customer).Include(invoice => invoice.Lines).ToListAsync();
    }
}
