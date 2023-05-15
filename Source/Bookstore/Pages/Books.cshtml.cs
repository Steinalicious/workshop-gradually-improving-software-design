﻿using Bookstore.Data;
using Bookstore.Data.Seeding;
using Bookstore.Data.Specifications;
using Bookstore.Domain.Models;
using Bookstore.Domain.Models.BibliographicFormatters;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BooksModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IUnitOfWork _dbContext;
    public IEnumerable<(Guid id, Citation authors, Citation title)> Books { get; private set; } = Enumerable.Empty<(Guid, Citation, Citation)>();
    public IReadOnlyList<string> PublishedAuthorInitials { get; private set; } = Array.Empty<string>();
    private readonly IDataSeed<Book> _booksSeed;
    private readonly ISpecification<Book> _spec;
    private readonly IAuthorListFormatter _authorListFormatter;

    public BooksModel(ILogger<IndexModel> logger, IUnitOfWork dbContext, IDataSeed<Book> booksSeed, ISpecification<Book> spec, IAuthorListFormatter authorListFormatter) =>
        (_logger, _dbContext, _booksSeed, _spec, _authorListFormatter) = (logger, dbContext, booksSeed, spec, authorListFormatter);

    public async Task OnGet([FromQuery] string? initial, [FromQuery] string? authorId)
    {
        await this._booksSeed.SeedAsync();
        await this.PopulatePublishedAuthorInitials();
        await this.PopulateBooks(initial, TryParseGuid(authorId));
    }

    private async Task PopulatePublishedAuthorInitials() =>
        this.PublishedAuthorInitials  = await ((BookstoreDbContext)_dbContext).BookAuthors      // Requires redefinition when specifications are implemented
            .GetPublishedAuthors()
            .Select(author => author.LastName.Substring(0, 1))
            .Distinct()
            .OrderBy(initial => initial)
            .ToListAsync();

    private async Task PopulateBooks(string? authorInitial, Guid? authorId)
    {
        ISpecification<Book> spec = _spec;
        if (authorInitial is not null) spec = spec.ByAuthorInitial(authorInitial);
        if (authorId is not null) spec = spec.ByAuthorId(authorId.Value);

        IBibliographicEntryFormatter bookFormatter = new TitleOnlyFormatter();
        this.Books = (await _dbContext.Books.QueryAsync(spec.OrderByTitle()))
            .ConvertAll(book => (book.Id, _authorListFormatter.ToCitation(book.Authors), bookFormatter.ToCitation(book)));
    }

    private static Guid? TryParseGuid(string? id) =>
        Guid.TryParse(id, out Guid result) ? result : null;
}
