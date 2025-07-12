using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure;
public class UserService : IUserService
{
    private readonly IServiceProvider _serviceProvider;
    public UserService(IServiceProvider serviceProvider) 
    {
        this._serviceProvider = serviceProvider;
    }//
    public bool IsLoggedIn(ClaimsPrincipal user)
    {
        return user?.Identity != null && user.Identity.IsAuthenticated;
    }
    public string GetUserId(ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApplicationUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal user)
    {
        //I don't want to use this if possible
        var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var id = GetUserId(user);

        if(id == null)
            return null;
        //Get the user
        return await userManager.FindByIdAsync(id);
    }//GetCurrentLoggedInUserAsync
}//class