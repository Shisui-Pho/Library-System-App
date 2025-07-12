using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    IEnumerable<Order> GetUserOrders(ApplicationUser user);
}