using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

//This is a model that will record all the information that is passed
// by the user when they want to contact the library.
public class ContactUs
{
    public int Id { get; set; }
    [Required]
    public string FullNames { get; set; }
    [Required]
    [RegularExpression("/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$/;")]
    public string Email {  get; set; }
    public string PhoneNumber { get; set; }
    [Required]
    public string Message {  get; set; }
    public bool Viewed { get; set; } = false;
}
