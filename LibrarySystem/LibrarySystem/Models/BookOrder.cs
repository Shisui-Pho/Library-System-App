using LibrarySystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;
public class BookOrder
{
    public int BookOrderId { get; set; }
    [ForeignKey(nameof(Book))]
    public int BookID { get; set; }
    public string UserID { get; set; }
    [StringLength(500, ErrorMessage ="Description cannot be more than 500 characters.")]
    public string Description { get; set; }
    public decimal TotalPrice => Quantity * 5m;//For now until we add price tag to the book
    [Required]
    [Range(1,5)]//Limit to 5 books per book order.
    public int Quantity { get; set; }
    public BookOrderStatus Status{ get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    //Naviagtion properties
    public Book Book { get; set; }
    [NotMapped]
    public ApplicationUser User { get; set; }
}