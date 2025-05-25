using LibrarySystem.Models.ViewModels;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public static class SeedIdentity
{
    //Application will have 3 roles
    //- Customer, Staff and Administrator
    private class MockRegisterViewModel : RegisterViewModel
    {
        //Define extra property for a role
        public string UserRole { get; set; }
    }
    private static readonly string[] roles = ["Customer", "Staff", "Administrator"];
    private static readonly List<MockRegisterViewModel> seedUsers =
    [
        new MockRegisterViewModel
    {
        FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Password = "A1ice@2024",
            ConfirmPassword = "A1ice@2024", ContactNumber = "123-456-7890", UserRole = roles[0]
    },
    new MockRegisterViewModel
    {
        FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", Password = "B0bSecure!",
        ConfirmPassword = "B0bSecure!", ContactNumber = "234-567-8901", UserRole = roles[1]
    },
    new MockRegisterViewModel
    {
        FirstName = "Clara", LastName = "Lee", Email = "clara.lee@example.com", Password = "Cl@ra2025",
        ConfirmPassword = "Cl@ra2025", ContactNumber = "345-678-9012", UserRole = roles[2]
    }
    ];

    public static async void PopulateLibraryUsersDB(IApplicationBuilder app)
    {
        UserManager<ApplicationUser> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
        RoleManager<IdentityRole> roleManager = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();

        //Get the dbcontext
        AppIdentityDBContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<AppIdentityDBContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        //Check the roles database if the roles exist
        if (!context.Roles.Any())
        {
            //Create the roles
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!context.Users.Any())
        {
            foreach (var seedUser in seedUsers)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = seedUser.Email,
                    PhoneNumber = seedUser.ContactNumber,
                    UserName = seedUser.Email, //The username is the same as their email
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName
                };
                await userManager.CreateAsync(user, seedUser.Password);
                await userManager.AddToRoleAsync(user, seedUser.UserRole);
            }
        }

        context.SaveChanges();
    }//PopulateLibraryUsersDB
}
