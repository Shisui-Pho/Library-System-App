using System.Linq;
namespace LibrarySystem.Models.ViewModels;
public class BooksDisplayViewModel(List<(int bookId, int quantity)> cartItems)
{
    public IEnumerable<Book> Books { get; set; }
    public IEnumerable<string> Genres { get; set; } = ["Horror", "Scifi", "Comedy"];
    public PagingInfomation PagingInfomation { get; set; }
    public CartItemStatus CartItemStatus { get; } = new CartItemStatus(cartItems);
    public FilteringOptions FilteringOptions { get; set; } = new FilteringOptions();
}//class
