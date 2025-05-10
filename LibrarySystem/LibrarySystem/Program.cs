using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure midlle ware
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//Route mapping
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

//Add test data
SeedData.EnsurePopulated(app);

app.Run();