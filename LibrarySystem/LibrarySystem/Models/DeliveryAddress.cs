using LibrarySystem.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;
public class DeliveryAddress
{
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    // Delivery Address
    [Required(ErrorMessage = "Address line 1 is required")]
    [DisplayName("Address Line 1")]
    public string AddressLine1 { get; set; }
    [DisplayName("Address Line 2")]
    public string AddressLine2 { get; set; }
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    [Required(ErrorMessage = "Postal code is required")]
    [RegularExpression(@"\d{4}", ErrorMessage = "Postal code must be 4 digits")]
    [DisplayName("Postal Code")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Province is required")]
    public string Province { get; set; }
}
