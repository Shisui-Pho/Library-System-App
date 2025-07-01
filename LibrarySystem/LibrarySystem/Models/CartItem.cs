using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;
public class CartItem
{
    public int CartItemID { get; set; }
    [Required]
    [ForeignKey(nameof(Book))]
    public int BookID {  get; set; }
    public string UserID { get; set; }
    [Required]
    [Range(1,10)]
    public int Quantity { get; set; }
    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }
    [Required]
    public DateOnly CreatedDate { get; set; }
    [Required]
    public bool HasBeenCheckedOut { get; set; } = false;

    //Navigational properties
    public Book BookInCart { get; set; }
    public decimal GetTotalPrice()
    {
        return Price * Quantity;
    }//GetTotalPrice
}//class