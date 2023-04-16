using Bookstore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Domain.Specifications;

public static class BookSpecifications
{
    public static IQueryable<Book> GetBooks(this IQueryable<Book> books) =>
        books
            .Include(book => book.AuthorsCollection)
            .ThenInclude(bookAuthor => bookAuthor.Person);
}