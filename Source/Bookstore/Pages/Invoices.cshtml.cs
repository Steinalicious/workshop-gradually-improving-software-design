using Bookstore.Data.Seeding;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Pages;

public class InvoicesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _context;
    private readonly IDataSeed<Invoice> _invoicesSeed;

    public InvoicesModel(ILogger<IndexModel> logger, BookstoreDbContext context, IDataSeed<Invoice> invoicesSeed) => 
        (_logger, _context, _invoicesSeed) = (logger, context, invoicesSeed);

    public async Task OnGet()
    {
        await _invoicesSeed.SeedAsync();
    }
}
