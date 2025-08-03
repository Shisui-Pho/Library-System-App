using LibrarySystem.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;
public class AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) 
             : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<LoggedInUser> loggedInUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<LoggedInUser>().ToTable(nameof(LoggedInUser));
        base.OnModelCreating(builder);
    }
}
