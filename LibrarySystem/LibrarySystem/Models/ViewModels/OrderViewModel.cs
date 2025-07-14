using LibrarySystem.Infrastructure.Enums;

namespace LibrarySystem.Models.ViewModels;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalPrice { get; set; }
    public BookOrderStatus Status { get; set; }
    public DeliveryOption DeliveryOption { get; set; }

    // For first book display
    public string FirstBookTitle { get; set; }
    public string FirstBookAuthor { get; set; }
    public string FirstBookCover { get; set; }
}
