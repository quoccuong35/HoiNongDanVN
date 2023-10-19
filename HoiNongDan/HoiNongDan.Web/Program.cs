using Microsoft.EntityFrameworkCore;
using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbpass = builder.Configuration.GetSection("DBPass").Value;

var builderSQL = new System.Data.SqlClient.SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
builderSQL.Password = dbpass;

string defaultConnection = builderSQL.ConnectionString;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(defaultConnection);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.SlidingExpiration = true;
    options.LoginPath = $"/Permission/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
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
    options.IdleTimeout = TimeSpan.FromDays(30);
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
