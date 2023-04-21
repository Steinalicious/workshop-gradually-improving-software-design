using Bookstore.Domain.Common;

namespace Bookstore.Domain.Models;

public record BookPrice(Book Book, Money Value)
{
    public override string ToString() => $"[{Book.Title}] {Value}";
}