using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taply.Models;
using Taply.Services;

var contentRoot = Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
    ? Directory.GetCurrentDirectory()
    : AppContext.BaseDirectory;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = contentRoot,
    WebRootPath = Path.Combine(contentRoot, "wwwroot")
});
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TaplyDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("TaplyDb")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o => { o.LoginPath = "/admin/login"; o.AccessDeniedPath = "/admin/login"; o.Cookie.Name = "Taply.Admin"; });
builder.Services.AddAuthorization();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseStaticFiles(); app.UseRouting(); app.UseAuthentication(); app.UseAuthorization();
app.MapControllerRoute("public-detail", "p/{slug}/{sectionSlug}", new { controller = "Public", action = "Section" });
app.MapControllerRoute("public-page", "p/{slug}", new { controller = "Public", action = "Page" });
app.MapControllerRoute("qr", "qr/{slug}", new { controller = "Public", action = "Qr" });
app.MapControllerRoute("admin", "admin/{controller=Places}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope()) { var db = scope.ServiceProvider.GetRequiredService<TaplyDbContext>(); await db.Database.MigrateAsync(); await SeedData.Initialize(db); }
app.Run();
