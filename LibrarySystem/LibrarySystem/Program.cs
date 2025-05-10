var builder = WebApplication.CreateBuilder(args);

//Configure midlle ware
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//Route mapping
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
