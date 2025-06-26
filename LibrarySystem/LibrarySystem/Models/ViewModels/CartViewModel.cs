namespace LibrarySystem.Models.ViewModels;
public class CartViewModel
{
    public IEnumerable<CartItem> CartItems { get; set; }
    public int SubTotalItems => CartItems?.Select(x => x.Quantity).Sum() ?? 0;
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
        decimal price = CartItems.Select(x => x.Quantity * x.Price).Sum();
        return price;
    }
}//class
