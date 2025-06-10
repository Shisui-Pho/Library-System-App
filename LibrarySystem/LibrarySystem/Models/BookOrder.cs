using LibrarySystem.Enums;

namespace LibrarySystem.Models;
public class BookOrder
{
    public int Id { get; set; }
    public int BookID { get; set; }
    public string UserID { get; set; }
    public string Description { get; set; }
    public decimal TotalPrice => 5m;
    public int Quantity { get; set; }
    public DateTime OrderPlacedDate { get; set; }
    public BookOrderStatus Status{ get; set; }

    //Naviagtion properties
    public Book Book { get; set; }
    public ApplicationUser User { get; set; }
}