using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models.Identity;
public class LoggedInUser
{
    public int Id { get; set; }
    [ForeignKey("AspNetUsers")]
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public string IPAddress { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LastActivity { get; set; }
    //Navigational property
    public ApplicationUser User { get; set; }
}//class