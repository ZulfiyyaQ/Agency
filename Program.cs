using Agency.DAL;
using Agency.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<LayoutService>();
var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    "Agency",
    "{controller=home}/{action=index}/{id?}"
    );

app.Run();
