using Bookstore.Data.Seeding;
using Bookstore.Data.Seeding.DataSeed;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string bookstoreConnectionString =
    builder.Configuration.GetConnectionString("BookstoreConnection") ?? string.Empty;
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(bookstoreConnectionString));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IDataSeed<Person>, AuthorsSeed>();
    builder.Services.AddScoped<IDataSeed<Book>, BooksSeed>();
}
else
{
    builder.Services.AddScoped<IDataSeed<Person>, NoSeed<Person>>();
    builder.Services.AddScoped<IDataSeed<Book>, NoSeed<Book>>();
}

decimal relativeDiscount = builder.Configuration.GetValue<decimal>("Discounts:RelativeDiscount", 0);
builder.Services.AddSingleton<IDiscount>(_ => (new RelativeDiscount(relativeDiscount).Then(new TitlePrefixDiscount(.15M, "C"))).CapTo(.20M));

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
