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
}
