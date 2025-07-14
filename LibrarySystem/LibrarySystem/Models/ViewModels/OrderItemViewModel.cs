namespace LibrarySystem.Models.ViewModels;
public class OrderItemViewModel
{
    public string BookTitle { get; set; }
    public string Authors { get; set; }
    public string BookCoverUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}