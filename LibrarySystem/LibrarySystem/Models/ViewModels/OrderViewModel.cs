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
    public OrderViewModel(Order order)
    {
        AssignProperties(order);
    }
    private void AssignProperties(Order order)
    {
        var firstBookItem = order.BookOrderItems.FirstOrDefault();
        var firstBook = firstBookItem?.Book;
        var firstAuthor = firstBook?.Authors.FirstOrDefault();

        OrderId = order.OrderId;
        OrderNumber = $"ORD-{order.OrderId:D1}";
        OrderDate = order.CreatedAt;
        ItemCount = order.BookOrderItems.Count;
        TotalPrice = order.TotalPrice;
        Status = order.Status;
        DeliveryOption = order.DeliveryOption;
        FirstBookTitle = firstBook?.BookTitle;
        FirstBookAuthor = firstAuthor != null ? $"{firstAuthor.FullName} {firstAuthor.LastName}" : "Unknown";
        FirstBookCover = firstBook?.GetCoverPath();
    }//AssignProperties
}//class
