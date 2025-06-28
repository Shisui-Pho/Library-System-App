using System.Linq;
namespace LibrarySystem.Models.ViewModels;
public class BooksDisplayViewModel
{
    private readonly List<(int bookId, int quantity)> cartItems;
    public IEnumerable<Book> Books { get; set; }
    public PagingInfomation PagingInfomation { get; set; }
    public BooksDisplayViewModel(List<(int bookId, int quantity)> cartItems)
    {
        this.cartItems = cartItems;
    }

    public string GenerateUniqueIdentifier(int bookID, bool isForAddToChart = false)
    {
        return isForAddToChart ? "addbookNumber" + bookID : "updatebookNumber" + bookID;
    }
    public (bool isContained, int quantity) IsBookInCart(int bookID)
    {
        if (cartItems == null)
            return (false, 0);

        var index = cartItems.FindIndex(item => item.bookId == bookID);

        if (index >= 0)
            return (true, cartItems[index].quantity);

        //return false
        return (false, 0);
    }//IsBookInCart
}//class