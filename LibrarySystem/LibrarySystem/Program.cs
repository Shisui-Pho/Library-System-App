using LibrarySystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure midlle ware
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("LibraryDatabase")));
builder.Services.AddDbContext<AppIdentityDBContext>(options => 
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("LibraryUsersDatabase")));

//Inject repositoy wrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

//-Configure route url
builder.Services.AddRouting(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = true;//this makes sure my url is always lower case for readability
});

//-Configure authorisation paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LoginPath = "/Account/Login";
});

//-Configure password and security options for the user credentials

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;//This ensures that all users have unique username since we will
                                            // their emails
});



var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//-Route mapping
app.MapControllerRoute(
    name: "ListPage",
    pattern: "{controller}/{action}/page-{page}");

app.MapControllerRoute(
    name: "ListPageTag",
    pattern: "{controller}/{action}/{tag}/page-{page}");

app.MapControllerRoute(
    name: "Details",
    pattern: "{controller}/{action}/{id}/{slug}");

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

//Add test data
SeedData.EnsurePopulated(app);

app.Run();