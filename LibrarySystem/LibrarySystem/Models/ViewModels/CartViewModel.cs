namespace LibrarySystem.Models.ViewModels;
public class CartViewModel
{
    public IEnumerable<CartItem> CartItems { get; set; }
    public int SubTotalItems => CartItems?.Select(x => x.Quantity).Sum() ?? 0;

    public decimal CalculateTotalPrice()
    {
        decimal price = CartItems.Select(x => x.Quantity * x.Price).Sum();
        return price;
    }
    public (bool isContained, int quantity) ContainsBook(int bookID)
    {
        if(CartItems == null)
            return (false, 0);

        var item = CartItems.FirstOrDefault(item => item.BookID == bookID);

        if (item != null)
            return (true, item.Quantity);

        //return false
        return (false, 0);
    }//ContainsBook
}//class