using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Enums;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace LibrarySystem.Infrastructure;
public class OrderService : IOrderService
{
    private readonly IUserService _userService;
    private readonly IRepositoryWrapper _repo;
    public OrderService(IUserService userService,IRepositoryWrapper repo)
    {
        this._userService = userService;
        this._repo = repo;
    }
    public bool CancelOrder(ClaimsPrincipal user, int orderID)
    {
        //Find the by ID
        var userId = _userService.GetUserId(user);
        if (userId == null)
        {
            return false; // User not found
        }
        var order = _repo.Orders.FindByCondition(o => o.OrderId == orderID && o.UserID == userId).FirstOrDefault();

        if(order == null)
            return false; // Order not found or does not belong to user

        if (order.Status != BookOrderStatus.Pending)
            return false; // Order cannot be cancelled if not pending

        // Update the order status to Cancelled
        order.Status = BookOrderStatus.Cancelled;
        order.LastUpdatedAt = DateTime.UtcNow;
        order.Description = "Order cancelled by user.";
        _repo.Orders.Update(order);
        try
        {
            _repo.SaveChanges();
            return true; //Order cancelled successfully
        }
        catch (DbUpdateException)
        {
            return false; //Failed to cancel order
        }
    }//CancelOrder

    public OrderDetailsViewModel GetOrderDetails(ClaimsPrincipal user, int orderDetails)
    {
        return MakeOrderDetails(user, orderDetails).Result;
    }

    public IEnumerable<OrderViewModel> GetOrders(ClaimsPrincipal user, QueryOptions<Order> options = null)
    {
        var orderUser = _userService.GetCurrentLoggedInUserAsync(user).Result;

        var orders = _repo.Orders.GetUserOrders(orderUser, options);

        // Project to ViewModel
        var orderViewModels = orders.Select(o => new OrderViewModel(o)).ToList();

        return orderViewModels;
    }
    private async Task<OrderDetailsViewModel> MakeOrderDetails(ClaimsPrincipal user, int id)
    {
        var orderUser = await _userService.GetCurrentLoggedInUserAsync(user);

        if(orderUser == null)
        {
            return null; // User not found
        }

        var order = _repo.Orders.GetOrderWithDetails(orderUser, id);
       
        var model = new OrderDetailsViewModel(order)
        {
            ShippingCost = order.DeliveryOption == DeliveryOption.Delivery ? 50.00m : 0.00m,
            EstimatedDeliveryDate = DateTime.Now.AddDays(3),
            TrackingNumber = "SA" + new Random().Next(1000000, 9999999).ToString(),
        };
        return model;
    }//MakeOrderDetails
}//class