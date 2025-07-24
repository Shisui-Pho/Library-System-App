using System.Linq;
namespace LibrarySystem.Models.ViewModels;
public class BooksDisplayViewModel(List<(int bookId, int quantity)> cartItems)
{
    private BookFilteringOptions _filteringOptions = new BookFilteringOptions();
    public IEnumerable<Book> Books { get; set; }
    public IEnumerable<string> Genres { get; set; } = ["Horror", "Scifi", "Comedy"];
    public PagingInfomation PagingInfomation { get; set; }
    public CartItemStatus CartItemStatus { get; } = new CartItemStatus(cartItems);
    public BookFilteringOptions FilteringOptions 
    { 
        get => _filteringOptions;
        set
        {
            PagingInfomation = value?.Paging ?? new PagingInfomation();
            _filteringOptions = value;
        }
    }//end properyty
}//class
