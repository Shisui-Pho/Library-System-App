using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;

public interface IOrderRepository : IBaseRepository<BookOrder>
{
    IEnumerable<BookOrder> GetUserOrders(ApplicationUser user);
}