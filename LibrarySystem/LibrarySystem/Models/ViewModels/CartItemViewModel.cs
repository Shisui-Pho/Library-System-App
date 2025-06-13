namespace LibrarySystem.Models.ViewModels;
public class CartItemViewModel
{
    public CartItem CartItem { get; set; }

    //This will be the page that the user clicked on to add item to cart
    //-The item will be linked to the fragment labeled: product-id
    public string PageWhereItemWasAdded { get; set; }

}
