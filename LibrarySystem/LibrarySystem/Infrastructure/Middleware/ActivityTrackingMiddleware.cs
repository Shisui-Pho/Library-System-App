using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace LibrarySystem.Infrastructure.Middleware;
public class ActivityTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public ActivityTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        if (userService.IsLoggedIn())
        {
            await userService.UpdateUserActivityStatus(context);
        }
        await _next(context);
    }//Invoke
}//class
