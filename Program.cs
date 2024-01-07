using Agency.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.MapControllerRoute(
    "Agency",
    "{controller=home}/{action=index}/{id?}"
    );

app.Run();
