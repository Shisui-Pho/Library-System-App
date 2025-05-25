using LibrarySystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) 
             : IdentityDbContext<ApplicationUser>(options)
{
}
