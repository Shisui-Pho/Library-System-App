namespace LibrarySystem.Infrastructure.Enums;
public enum BookOrderStatus
{
    Pending,    // Order has been placed but not yet processed
    Processing, // Order is being prepared
    Shipped,    // Order has been shipped to the customer
    Delivered,  // Order has been delivered
    Cancelled,  // Order was cancelled before being shipped
    Returned,   // Order was returned by the customer
    Refunded    // Order amount has been refunded
}