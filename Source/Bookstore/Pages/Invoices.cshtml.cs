using Bookstore.Data.Seeding;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class InvoicesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<Invoice> _invoicesSeed;

    public IEnumerable<Invoice> Invoices { get; private set; } = Enumerable.Empty<Invoice>();

    public InvoicesModel(ILogger<IndexModel> logger, BookstoreDbContext context, IDataSeed<Invoice> invoicesSeed) => 
        (_logger, _context, _invoicesSeed) = (logger, context, invoicesSeed);

    public async Task OnGet()
    {
        await _invoicesSeed.SeedAsync();

        this.Invoices = await _context.Invoices.Include(invoice => invoice.Customer).Include(invoice => invoice.Lines).ToListAsync();
    }
}
