using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;
public class OrderRepository(AppDBContext context) : BaseRepository<BookOrder>(context), IOrderRepository
{
    public IEnumerable<BookOrder> GetUserOrders(ApplicationUser user)
    {
        return base._dbContext.BookOrders.Where(b => b.UserID == user.Id);
    }//GetUserOrders
}//class
