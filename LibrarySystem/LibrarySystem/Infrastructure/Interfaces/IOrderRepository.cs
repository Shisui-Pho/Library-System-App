using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;

public interface IOrderRepository : IBaseRepository<BookOrder>
{
    IEnumerable<BookOrder> GetUserOrders(ApplicationUser user);
}