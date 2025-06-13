using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;
public class CartItem
{
    public int CartItemID { get; set; }
    [ForeignKey(nameof(Book))]
    public int BookID {  get; set; }

    //Navigational property
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}