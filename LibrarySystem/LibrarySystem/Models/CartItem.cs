using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;
public class CartItem
{
    public int CartItemID { get; set; }
    [ForeignKey(nameof(Book))]
    public int BookID {  get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateOnly CreatedDate { get; set; }

    //Navigational property
    public Book BookInCart { get; set; }
    public decimal GetTotalPrice()
    {
        return Price * Quantity;
    }//GetTotalPrice
}//class