using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IOrderService
{
    IEnumerable<OrderViewModel> GetOrders(ClaimsPrincipal user,QueryOptions<Order> options = null);
    OrderDetailsViewModel GetOrderDetails(ClaimsPrincipal user, int orderDetails);
    bool CancelOrder(ClaimsPrincipal user, int orderID);
}//class
