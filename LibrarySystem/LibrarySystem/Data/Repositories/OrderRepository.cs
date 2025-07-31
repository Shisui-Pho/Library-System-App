using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories;
public class OrderRepository(AppDBContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public IEnumerable<Order> GetUserOrders(ApplicationUser user, QueryOptions<Order> options = null)
    {
        options ??= new QueryOptions<Order>(){Where = p => p.UserID == user.Id};
        options.Where ??= o => o.UserID == user.Id;
        //Apply query options
        var qOrders = base.ApplyOptions(options);

        //Include extra details
        qOrders = qOrders.Include(o => o.BookOrderItems)
                             .ThenInclude(boi => boi.Book)
                             .ThenInclude(b => b.Authors);
        return qOrders;
    }//GetUserOrders
    public Order GetOrderWithDetails(ApplicationUser user, int orderID)
    {
        var options = new QueryOptions<Order>() { Where = p => p.UserID == user.Id };
        var order = base.ApplyOptions(options)
                        .Include(o => o.BookOrderItems)
                            .ThenInclude(oi => oi.Book)
                        .Include(o => o.DeliveryAddress)
                        .Include(o => o.PickupPoint)
                        .FirstOrDefault();

        //Add the payment method:
        if(order  != null)
        {
            var paymentMethod = base._dbContext.PaymentMethods.Find(order.PaymentMethodId);
            order.PaymentMethod = paymentMethod;
            order.User = user;
        }
        return order;
    }//GetOrderWithDetails
}//class
