using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage ="Please provide your first names.")]
    [MinLength(1)]
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [Required(ErrorMessage ="Please provide your surname.")]
    [MinLength(1)]
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [Required(ErrorMessage ="An email address is needed.")]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Length(2,100, ErrorMessage ="The length of the password must be between 2 and 100 characters.")]
    public string Password { get; set; }
    [Required]
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; }
    [DisplayName("Contact Number")]
    public string ContactNumber { get; set; } 
}
