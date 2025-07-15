using AspNetCoreGeneratedDocument;
using LibrarySystem.Infrastructure.Enums;
namespace LibrarySystem.Models.ViewModels;

public class OrderDetailsViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public BookOrderStatus Status { get; set; }

    // User Information
    public string FullName { get; set; }
    public string Email { get; set; }

    // Delivery Information
    public DeliveryOption DeliveryOption { get; set; }
    public DeliveryAddress DeliveryAddress { get; set; }
    public PickupPoint PickupPoint { get; set; }

    // Payment Information
    public PaymentMethod PaymentMethod { get; set; }

    // Order Items
    public List<OrderItemViewModel> OrderItems { get; set; }

    // Totals
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }

    // Tracking Information
    public string TrackingNumber { get; set; }
    public string TrackingUrl { get; set; }
    public OrderDetailsViewModel(Order order)
    {
        AssignProperties(order);
    }
    private void AssignProperties(Order order)
    {
        OrderId = order.OrderId;
        OrderNumber = $"ORD-{order.OrderId:D6}";
        OrderDate = order.CreatedAt;
        Status = order.Status;
        FullName = $"{order.User.FirstName} {order.User.LastName}";
        Email = order.User.Email;
        DeliveryOption = order.DeliveryOption;
        DeliveryAddress = order.DeliveryAddress;
        PickupPoint = order.PickupPoint;
        PaymentMethod = order.PaymentMethod;
        OrderItems = [.. order.BookOrderItems.Select(MakeOrderItem)];
        Subtotal = order.BookOrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
        TotalPrice = order.TotalPrice;
        TrackingUrl = "https://tracking.sapost.co.za/?tracking=";
    }
    private OrderItemViewModel MakeOrderItem(OrderItem oi)
    {
        return new OrderItemViewModel
        {
            BookTitle = oi.Book.BookTitle,
            Authors = oi.Book.GetAuthorsString(),
            BookCoverUrl = oi.Book.GetCoverPath(),
            Price = oi.UnitPrice,
            Quantity = oi.Quantity,
            TotalPrice = oi.UnitPrice * oi.Quantity
        };
    }//MakeOrderItem
}