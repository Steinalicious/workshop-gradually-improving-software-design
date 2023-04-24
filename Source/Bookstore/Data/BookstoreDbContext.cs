using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => base.Set<Book>();
    public DbSet<BookAuthor> BookAuthors => base.Set<BookAuthor>();

    private DbSet<BookPrice> BookPrices => base.Set<BookPrice>();

    public IQueryable<BookPrice> GetBookPricesAt(DateTime time) => this.BookPrices
        .Where(price => price.ValidFrom <= time)
        .GroupBy(price => price.BookId)
        .Select(group => group.OrderByDescending(price => price.ValidFrom).First());

    public DbSet<Person> People => base.Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Books");

        modelBuilder.Entity<Person>()
            .Property(person => person.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BookAuthor>()
            .HasKey(new[] { "BookId", "PersonId" });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(bookAuthor => bookAuthor.Book)
            .WithMany("AuthorsCollection")
            .HasForeignKey("BookId");

        modelBuilder.Entity<BookAuthor>()
            .HasOne(bookAuthor => bookAuthor.Person)
            .WithMany();

        modelBuilder.Entity<Book>()
            .Property(book => book.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Book>()
            .Ignore(book => book.Authors);

        modelBuilder.Entity<BookPrice>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(price => price.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookPrice>().Ignore(bookPrice => bookPrice.Price);

        modelBuilder.Entity<BookPrice>()
            .Property<decimal>("Amount")
            .HasColumnName("Amount")
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<BookPrice>()
            .Property<Currency>("Currency")
            .HasColumnName("Currency")
            .HasConversion(
                currency => currency.Symbol,
                symbol => new Currency(symbol))
            .IsRequired();
    }
}