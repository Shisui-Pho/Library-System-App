using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    IEnumerable<Order> GetUserOrders(ApplicationUser user, QueryOptions<Order> options = null);
    Order GetOrderWithDetails(ApplicationUser user, int orderID);
}