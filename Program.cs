using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyContext>(option =>
{
    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


