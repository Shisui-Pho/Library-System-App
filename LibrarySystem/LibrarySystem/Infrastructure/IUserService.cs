using System.Security.Claims;

namespace LibrarySystem.Infrastructure;
public interface IUserService
{
    string GetUserId(ClaimsPrincipal user);
    bool IsLoggedIn(ClaimsPrincipal user);
}