namespace LibrarySystem.Models.ViewModels;
public class CartViewModel
{
    public IEnumerable<BookOrder> BookOrders { get; set; }
    public int SubTotalItems => BookOrders?.Select(x => x.Quantity).Sum() ?? 0;
    public decimal CalculateTotalCartPriceWithDiscount(double discountPercentage = 0)
    {
        //Calculate the total price
        decimal totalPrice = CalculateTotalPrice();

        //Apply the discount
        totalPrice -= totalPrice * (decimal)discountPercentage;

        //returnt the discount price
        return totalPrice;
    }
    public decimal CalculateTotalPrice()
    {
        return BookOrders.Select(x =>x.TotalPrice).Sum();
    }
}//class
