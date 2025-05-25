using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    [MinLength(1)]
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [Required]
    [MinLength(1)]
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Length(2,100, ErrorMessage ="The length of the password must be between 2 and 100 characters")]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    public string ContactNumber { get; set; } 
}
