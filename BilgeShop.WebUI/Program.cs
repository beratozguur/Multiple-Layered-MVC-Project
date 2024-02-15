using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giriþ - çýkýþ - eriþim engeli durumlandýrýnda yönlendirilecek olan adresler.
});

var contentRoutePath = builder.Environment.ContentRootPath;

var keysDirector = new DirectoryInfo(Path.Combine(contentRoutePath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("Bilge Shop")
    .SetDefaultKeyLifetime(new TimeSpan(99999, 0, 0, 0))
    .PersistKeysToFileSystem()

var app = builder.Build();

app.UseStaticFiles(); // wwwroot için

app.UseAuthentication();
app.UseAuthorization();
// Auth iþlemleri yapýyorsan, üstteki 2 satýr yazýlmalý. Yoksa hata vermez fakat oturum açamaz, yetkilendirme sorgulayamaz.

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

// AREA ÝÇÝN YAZILAN ROUTE HER ZAMAN DEFAULT'UN ÜZERÝNDE OLACAK

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();





