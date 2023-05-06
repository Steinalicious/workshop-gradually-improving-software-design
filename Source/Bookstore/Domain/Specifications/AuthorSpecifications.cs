using Bookstore.Domain.Models;

namespace Bookstore.Domain.Specifications;

public static class AuthorSpecifications
{
    public static IQueryable<Person> GetPublishedAuthors(this IQueryable<BookAuthor> bookAuthors) =>
        bookAuthors.Select(bookAuthor => bookAuthor.Person).Distinct();
}