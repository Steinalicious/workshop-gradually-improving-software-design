using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => base.Set<Book>();
    public DbSet<BookAuthor> BookAuthors => base.Set<BookAuthor>();

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
    }
}