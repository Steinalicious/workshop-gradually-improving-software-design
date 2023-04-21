using System.Text;
using System.Security.Cryptography;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Common;

public static class BookPricing
{
    public static BookPrice SeedPriceFor(Book book, Currency currency)
    {
        byte[] title = Encoding.UTF8.GetBytes(book.Title);
        byte[] hashBytes = SHA256.HashData(title);
        int seed = ToInts(hashBytes).Aggregate((a, b) => a ^ b);
        Random random = new(seed);
        decimal price = random.Next(3000, 6000) / 100m;
        return new BookPrice(book, new Money(price, currency));
    }

    private static IEnumerable<int> ToInts(byte[] bytes) =>
        Enumerable.Range(0, bytes.Length / 4).Select(i => BitConverter.ToInt32(bytes, i * 4));
}