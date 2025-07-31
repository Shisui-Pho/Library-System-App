using LibrarySystem.Data;
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

    public async Task Invoke(HttpContext context, AppIdentityDBContext db)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sessionId = context.Session.GetString("SessionId");

            var record = await db.loggedInUsers
                .FirstOrDefaultAsync(x => x.UserId == userId && x.SessionId == sessionId);

            if (record != null)
            {
                record.LastActivity = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        await _next(context);
    }
}//class
