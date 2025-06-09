using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;
public class LogInViewModel
{
    public string UserName { get; set; }
    [Required(ErrorMessage ="Please enter your password.")]
    public string Password { get; set; }
    [Required(ErrorMessage ="An email address is needed.")]
    public string Email { get; set; }
    public string ReturnURL { get; set; }
}
