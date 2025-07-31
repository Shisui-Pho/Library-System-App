using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Background_services;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.Middleware;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.Models.Identity;
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
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<AppIdentityDBContext>();

//Inject 
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderService, OrderService>(); 
builder.Services.AddScoped<IBooksService, BooksService>();

//-Session
builder.Services.AddSession();

//To get the httpcontext
builder.Services.AddHttpContextAccessor();

//-Configure route url
builder.Services.AddRouting(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = false;//this makes sure my url is always lower case for readability
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

//Configure timeout for a session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//For stale-sessions cleanup
builder.Services.AddHostedService<SessionCleanupService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseSession();
app.UseMiddleware<ActivityTrackingMiddleware>();
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
SeedData.PopulateLibrarySystemDB(app);
SeedIdentity.PopulateLibraryUsersDB(app);
app.Run();