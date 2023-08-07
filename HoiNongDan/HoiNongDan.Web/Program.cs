using Microsoft.EntityFrameworkCore;
using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.SlidingExpiration = true;
    options.LoginPath = $"/Permission/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.AccessDeniedPath = "/Error/AccessDenied";
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Permission/Auth/Login";
    options.LogoutPath = $"/Permission/Auth/LogOut";
    options.AccessDeniedPath = $"/Error/AccessDenied";
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "Area",
    pattern: "{area=Permission}/{controller=Auth}/{action=index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}/{id?}");

app.Run();
