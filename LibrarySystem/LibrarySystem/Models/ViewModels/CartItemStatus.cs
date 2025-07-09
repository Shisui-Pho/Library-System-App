namespace LibrarySystem.Models.ViewModels;

public class CartItemStatus
{
    private readonly List<(int bookId, int quantity)> cartItems;
    public CartItemStatus(List<(int bookId, int quantity)> items)
    {
        cartItems = items;
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
}