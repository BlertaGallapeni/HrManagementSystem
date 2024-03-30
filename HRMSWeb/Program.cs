using HRMS.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using HRMS.Utility;
using Umbraco.Core.Composing.CompositionExtensions;
using Umbraco.Core.Services;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using HRMS.Models.Entities;
using HRMS.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using HRMSWeb.Helpers;
using System.Text.Json.Serialization;
using HRMSWeb.Hubs;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var en = new CultureInfo("en-US");
    en.NumberFormat.NumberDecimalSeparator = ".";
    en.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
    en.DateTimeFormat.LongTimePattern = "dd/MM/yyyy";
    en.DateTimeFormat.ShortTimePattern = "HH:mm";
    en.DateTimeFormat.LongTimePattern = "HH:mm";
    var al = new CultureInfo("sq-AL");
    al.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
    al.DateTimeFormat.LongTimePattern = "dd.MM.yyyy";
    al.DateTimeFormat.ShortTimePattern = "HH:mm";
    al.DateTimeFormat.LongTimePattern = "HH:mm";
    al.NumberFormat.NumberDecimalSeparator = ".";

    var supportedCultures = new[]
                  {
                   en,
                   al
                };

    options.DefaultRequestCulture = new RequestCulture(en, en);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; }); 
builder.Services.AddMvc()
               .AddViewLocalization(
                   LanguageViewLocationExpanderFormat.Suffix,
                   opts => { opts.ResourcesPath = "Resources"; })
               .AddDataAnnotationsLocalization();
        
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultUI().AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddTransient<IFileHelper, FileHelper>();
builder.Services.AddTransient<IThumbnailService, ThumbnailService>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Admin/Account/Login";
    options.LogoutPath = $"/Admin/Account/Logout";
    options.AccessDeniedPath = $"/Admin/Account/AccessDenied";
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    //options.Password.RequiredLength = 6;
    //options.Password.RequiredUniqueChars = 1;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
app.UseRouting();
SeedDatabase();
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<LeaveRequestsHub>("/leaveRequestsHub");
});
app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "MyArea", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "default", pattern: "{area=Admin}/{controller=Account}/{action=Login}/{id?}");
});

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}