using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;
public class BookOrderItem
{
    public int BookOrderItemId { get; set; }

    [ForeignKey(nameof(BookOrder))]
    public int BookOrderId { get; set; }

    [ForeignKey(nameof(Models.Book))]
    public int BookId { get; set; }

    [Range(1, 10)]
    public int Quantity { get; set; }

    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public BookOrder BookOrder { get; set; }

    public Book Book { get; set; }
}
