using LibrarySystem.Models.Identity;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IUserService
{
    string GetUserId(ClaimsPrincipal user);
    bool IsLoggedIn(ClaimsPrincipal user);
    Task<ApplicationUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal user);
    Task<bool> LogInUser(LogInViewModel logInViewModel);
    Task<IdentityResult> RegisterUser(RegisterViewModel registerViewModel);
    Task LogOutUser();
    Task UpdateUserActivityStatus(HttpContext context);
}