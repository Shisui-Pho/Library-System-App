using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

//This is a model that will record all the information that is passed
// by the user when they want to contact the library.
public class MessageRequest
{
    public int Id { get; set; }
    [Required]
    public string FullNames { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", 
        ErrorMessage ="Invalid email. Example of valid email address: library@example.com")]
    public string Email {  get; set; }
    [Length(10, 10, ErrorMessage ="South African phone numbers has 10-digits.")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Message {  get; set; }
    public bool Viewed { get; set; } = false;
}
