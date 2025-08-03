using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IOrderService
{
    int CountOrders(QueryOptions<Order> options = null);
    IEnumerable<OrderViewModel> GetOrders(QueryOptions<Order> options = null);
    OrderDetailsViewModel GetOrderDetails(int orderDetails);
    bool CancelOrder(int orderID);
}//class
