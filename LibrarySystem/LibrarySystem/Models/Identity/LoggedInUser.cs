using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models.Identity;
public class LoggedInUser
{
    public int Id { get; set; }
    [Required]
    [ForeignKey("AspNetUsers")]
    public string UserId { get; set; }
    [Required]
    public string SessionId { get; set; }
    [Required]
    public string IPAddress { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LastActivity { get; set; }
    //Navigational property
    public ApplicationUser User { get; set; }
}//class