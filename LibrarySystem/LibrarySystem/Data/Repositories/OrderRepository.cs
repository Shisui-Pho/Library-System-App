using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;
public class OrderRepository(AppDBContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public IEnumerable<Order> GetUserOrders(ApplicationUser user)
    {
        return base._dbContext.BookOrders.Where(b => b.UserID == user.Id);
    }//GetUserOrders
}//class
