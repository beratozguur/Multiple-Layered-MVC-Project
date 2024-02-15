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
    // giri� - ��k�� - eri�im engeli durumland�r�nda y�nlendirilecek olan adresler.
});

var contentRoutePath = builder.Environment.ContentRootPath;

var keysDirector = new DirectoryInfo(Path.Combine(contentRoutePath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("Bilge Shop")
    .SetDefaultKeyLifetime(new TimeSpan(99999, 0, 0, 0))
    .PersistKeysToFileSystem()

var app = builder.Build();

app.UseStaticFiles(); // wwwroot i�in

app.UseAuthentication();
app.UseAuthorization();
// Auth i�lemleri yap�yorsan, �stteki 2 sat�r yaz�lmal�. Yoksa hata vermez fakat oturum a�amaz, yetkilendirme sorgulayamaz.

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

// AREA ���N YAZILAN ROUTE HER ZAMAN DEFAULT'UN �ZER�NDE OLACAK

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();





