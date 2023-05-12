using Bookstore.Data.Seeding;
using Bookstore.Data.Seeding.DataSeed;
using Bookstore.Domain.Models;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Discounts.Implementation;
using Microsoft.EntityFrameworkCore;
using Bookstore.Domain.Invoices;
using Bookstore.Data;
using Bookstore.Data.Implementation;
using Bookstore.Domain.Models.BibliographicFormatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigurationManager configuration = builder.Configuration;

string bookstoreConnectionString =
    builder.Configuration.GetConnectionString("BookstoreConnection") ?? string.Empty;
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(bookstoreConnectionString));

builder.Services.AddScoped<IUnitOfWork, BookstoreDbContext>();

builder.Services.AddScoped(typeof(ISpecification<>), typeof(QueryableSpecification<>));

builder.Services.AddScoped<InvoiceFactory>(_ => new InvoiceFactory(
    DateOnly.FromDateTime(DateTime.UtcNow),
    configuration.GetValue<int>("Invoicing:ToleranceDays", 30),
    configuration.GetValue<int>("Invoicing:DelinquencyDays", 10)));

builder.Services.AddSingleton<IDiscount>(_ =>
    (new RelativeDiscount(0.15M).Then(new TitlePrefixDiscount(.10M, "C"))).And(new TitleContentDiscount(.25M, "Code")).CapTo(.30M));

builder.Services.AddSingleton<IAuthorListFormatter>(_ => new SeparatedAuthorsFormatter(new ShortNameFormatter(), ", ", " and "));
builder.Services.AddSingleton<IBibliographicEntryFormatter>(_ => TitleAndAuthorsFormatter.Academic());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IDataSeed<Person>, AuthorsSeed>();
    builder.Services.AddScoped<IDataSeed<Book>, BooksSeed>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, BookPricesSeed>();
    builder.Services.AddScoped<IDataSeed<Customer>, CustomersSeed>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, InvoicesSeed>();
}
else
{
    builder.Services.AddScoped<IDataSeed<Person>, NoSeed<Person>>();
    builder.Services.AddScoped<IDataSeed<Book>, NoSeed<Book>>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, NoSeed<BookPrice>>();
    builder.Services.AddScoped<IDataSeed<Customer>, NoSeed<Customer>>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, NoSeed<InvoiceRecord>>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
