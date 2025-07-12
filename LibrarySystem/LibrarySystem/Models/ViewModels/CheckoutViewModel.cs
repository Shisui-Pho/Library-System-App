using LibrarySystem.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;
public class CheckoutViewModel
{
    //Cart details will be contained here
    public CartViewModel Cart { get; set; }


    // User Information
    [Required(ErrorMessage = "First name is required")]
    [DisplayName("First Name")] 
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }


    // Delivery Options(if no pickup)
    [Required(ErrorMessage = "Please select delivery method")]
    [DisplayName("Delivery Method")]
    public string DeliveryOption { get; set; } = "Delivery";


    // Delivery Address
    [RequiredIf(nameof(DeliveryOption), "Delivery", ErrorMessage = "Address line 1 is required")]
    [DisplayName("Address Line 1")]
    public string AddressLine1 { get; set; }

    [DisplayName("Address Line 2")]
    public string AddressLine2 { get; set; }

    [RequiredIf(nameof(DeliveryOption), "Delivery", ErrorMessage = "City is required")]
    public string City { get; set; }

    [RequiredIf(nameof(DeliveryOption), "Delivery", ErrorMessage = "Postal code is required")]
    [RegularExpression(@"\d{4}", ErrorMessage = "Postal code must be 4 digits")]
    [DisplayName("Postal Code")]
    public string PostalCode { get; set; }

    [RequiredIf(nameof(DeliveryOption), "Delivery", ErrorMessage = "Province is required")]
    public string Province { get; set; }


    // Pickup Information(if needed/ if no delivery)
    [RequiredIf(nameof(DeliveryOption), "Pickup", ErrorMessage = "Please select a pickup point")]
    public int PickupPointId { get; set; }

    // Payment Method
    [Required(ErrorMessage = "Payment method is required")]
    public int PaymentMethodId { get; set; }

    // Address Saving
    public bool SaveAddress { get; set; } = true;

    // Terms and Conditions
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms and conditions")]
    public bool AgreeToTerms { get; set; }



    //Properties for populating dropdowns in the view
    public IEnumerable<SelectListItem> Provinces { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

}//class