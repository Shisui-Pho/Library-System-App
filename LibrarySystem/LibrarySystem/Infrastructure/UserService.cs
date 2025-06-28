using LibrarySystem.Infrastructure.Interfaces;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure;
public class UserService : IUserService
{
    public bool IsLoggedIn(ClaimsPrincipal user)
    {
        return user?.Identity != null && user.Identity.IsAuthenticated;
    }
    public string GetUserId(ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}//class