using LibrarySystem.Data;

namespace LibrarySystem.Infrastructure.Background_services;
public class SessionCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan sessionTimeout = TimeSpan.FromMinutes(20);

    public SessionCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppIdentityDBContext>();

            var expirationTime = DateTime.UtcNow - sessionTimeout;

            var expired = db.loggedInUsers
                .Where(x => x.LastActivity < expirationTime);

            db.loggedInUsers.RemoveRange(expired);
            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }//ExecuteAsync
}//class