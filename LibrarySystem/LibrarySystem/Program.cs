using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure midlle ware
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//Inject repositoy wrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddRouting(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = true;//this makes sure my url is always lower case for readability
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