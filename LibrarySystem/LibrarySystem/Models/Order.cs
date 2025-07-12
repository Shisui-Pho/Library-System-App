using LibrarySystem.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace LibrarySystem.Models;
public class Order
{
    public int BookOrderId { get; set; }
    [ForeignKey(nameof(Models.Book))]
    public string UserID { get; set; }
    [StringLength(500, ErrorMessage ="Description cannot be more than 500 characters.")]
    public string Description { get; set; }
    [Precision(18, 2)]
    public decimal TotalPrice { get; set; }
    public BookOrderStatus Status{ get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    //PaymentMethod
    [ForeignKey(nameof(Models.PaymentMethod))]
    [Required]
    public int PaymentMethodId { get; set; }

    //DeliveryOptions
    [Required]
    public DeliveryOption DeliveryOption { get; set; }

    [ForeignKey(nameof(Models.DeliveryAddress))]
    [AllowNull]
    public int? DeliveryAddressId { get; set; }
    [ForeignKey(nameof(Models.PickupPoint))]
    [AllowNull]
    public int? PickupPointId { get; set; }

    //Navigation properties
    public ICollection<OrderItem> BookOrderItems { get; set; }

    //Either DeliveryAddress or PickupPoint must be set, but not both.
    // This is enforced at the application level, not in the database.
    public DeliveryAddress DeliveryAddress { get; set; }
    public PickupPoint PickupPoint { get; set; }
    [NotMapped]
    public ApplicationUser User { get; set; }
}