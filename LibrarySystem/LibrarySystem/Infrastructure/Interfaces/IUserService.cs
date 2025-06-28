using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IUserService
{
    string GetUserId(ClaimsPrincipal user);
    bool IsLoggedIn(ClaimsPrincipal user);
}