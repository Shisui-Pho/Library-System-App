using LibrarySystem.Data;
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
    private readonly AppDBContext _context;

    public OrderService(IUserService userService, AppDBContext context)
    {
        this._userService = userService;
        this._context = context;

    }
    public bool CancelOrder(ClaimsPrincipal user, int orderID)
    {
        //Find the by ID
        var userId = _userService.GetUserId(user);
        if (userId == null)
        {
            return false; // User not found
        }
        var order = _context.BookOrders
            .FirstOrDefault(o => o.OrderId == orderID && o.UserID == userId);

        if(order == null)
            return false; // Order not found or does not belong to user

        if (order.Status != BookOrderStatus.Pending)
            return false; // Order cannot be cancelled if not pending

        // Update the order status to Cancelled
        order.Status = BookOrderStatus.Cancelled;
        order.LastUpdatedAt = DateTime.UtcNow;
        order.Description = "Order cancelled by user.";
        _context.BookOrders.Update(order);
        try
        {
            _context.SaveChanges();
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

    public IEnumerable<OrderViewModel> GetOrders(ClaimsPrincipal user)
    {
        return LoadOrders(user).Result;
    }
    private async Task<IEnumerable<OrderViewModel>> LoadOrders(ClaimsPrincipal user)
    {
        var userId = _userService.GetUserId(user);

        var orders = await _context.BookOrders
            .Where(o => o.UserID == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Take(10)
            .Include(o => o.BookOrderItems)
                .ThenInclude(boi => boi.Book)
                .ThenInclude(b => b.Authors)
            .ToListAsync();

        // Project to ViewModel
        var orderViewModels = orders.Select(order => MakeOrder(order)).ToList();

        return orderViewModels;
    }
    private async Task<OrderDetailsViewModel> MakeOrderDetails(ClaimsPrincipal user, int id)
    {
        var userId = _userService.GetUserId(user);

        var order = await _context.BookOrders
            .Include(o => o.BookOrderItems)
            .ThenInclude(oi => oi.Book)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.PickupPoint)
            .FirstOrDefaultAsync(o => o.OrderId == id && o.UserID == userId);

        if (order == null)
        {
            return null;
        }

        var paymentMethod = _context.PaymentMethods.Find(order.PaymentMethodId);

        var model = new OrderDetailsViewModel
        {
            OrderId = order.OrderId,
            OrderNumber = $"ORD-{order.OrderId:D6}",
            OrderDate = order.CreatedAt,
            Status = order.Status,
            FullName = $"{order.User.FirstName} {order.User.LastName}",
            Email = order.User.Email,
            DeliveryOption = order.DeliveryOption,
            DeliveryAddress = order.DeliveryAddress,
            PickupPoint = order.PickupPoint,
            PaymentMethod = paymentMethod,
            OrderItems = order.BookOrderItems.Select(oi => new OrderItemViewModel
            {
                BookTitle = oi.Book.BookTitle,
                Authors = oi.Book.GetAuthorsString(),
                BookCoverUrl = oi.Book.GetCoverPath(),
                Price = oi.UnitPrice,
                Quantity = oi.Quantity,
                TotalPrice = oi.UnitPrice * oi.Quantity
            }).ToList(),
            Subtotal = order.BookOrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
            ShippingCost = order.DeliveryOption == DeliveryOption.Delivery ? 50.00m : 0.00m,
            TotalPrice = order.TotalPrice,
            EstimatedDeliveryDate = DateTime.Now.AddDays(3),
            TrackingNumber = "SA" + new Random().Next(1000000, 9999999).ToString(),
            TrackingUrl = "https://tracking.sapost.co.za/?tracking="
        };
        return model;
    }//MakeOrderDetails
    private OrderViewModel MakeOrder(Order order)
    {
        var firstBookItem = order.BookOrderItems.FirstOrDefault();
        var firstBook = firstBookItem?.Book;
        var firstAuthor = firstBook?.Authors.FirstOrDefault();

        return new OrderViewModel
        {
            OrderId = order.OrderId,
            OrderNumber = $"ORD-{order.OrderId:D1}",
            OrderDate = order.CreatedAt,
            ItemCount = order.BookOrderItems.Count,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            DeliveryOption = order.DeliveryOption,
            FirstBookTitle = firstBook?.BookTitle,
            FirstBookAuthor = firstAuthor != null ? $"{firstAuthor.FullName} {firstAuthor.LastName}" : "Unknown",
            FirstBookCover = firstBookItem?.Book.GetCoverPath()
        };
    }//MakeOrder
}//class