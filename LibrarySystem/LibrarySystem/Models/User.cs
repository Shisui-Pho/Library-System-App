using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
