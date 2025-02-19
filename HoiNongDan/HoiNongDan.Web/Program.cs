using Microsoft.EntityFrameworkCore;
using HoiNongDan.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


// Rate limiter
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

int timeOut = int.Parse(builder.Configuration.GetSection("SiteSettings:TimeOutCookie").Value);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.Cookie.Name = "HND";
    options.LoginPath = $"/Permission/Auth/Login";
    options.AccessDeniedPath = "/Error/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(timeOut);
   
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Permission/Auth/Login";
    options.LogoutPath = $"/Permission/Auth/LogOut";
    options.AccessDeniedPath = $"/Error/AccessDenied";
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<FormOptions>(options => {
    options.ValueCountLimit = 8000;
});



builder.Services.AddMvcCore(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddDistributedMemoryCache();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(timeOut);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
