using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Invoices;

public interface IDelinquent
{
    IEnumerable<DueNotification> GetNotifications();
}

public record DueNotification(Customer Customer, Money Amount);
