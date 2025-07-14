using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IOrderService
{
    IEnumerable<OrderViewModel> GetOrders(ClaimsPrincipal user);
    OrderDetailsViewModel GetOrderDetails(ClaimsPrincipal user, int orderDetails);
    bool CancelOrder(ClaimsPrincipal user, int orderID);
}//class
