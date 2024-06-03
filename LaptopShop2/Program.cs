using LaptopShop2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using LaptopShop2;
using Microsoft.Extensions.Options;
using LaptopShop2.Functions;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LaptopShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Admin/LoginAdmin"; 
                options.LogoutPath = "/Admin/LoginAdmin/Logout"; 
                
            });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files")
    ),
    RequestPath = "/files"
});

app.UseSession();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();