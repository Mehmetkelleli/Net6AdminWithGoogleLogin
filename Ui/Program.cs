using Backend.Domain.EntityClass;
using Backend.Infrastructure.SignalR;
using Backend.Persistence.Context;
using Backend.Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;
builder.Services.AddServices();
//Identity Confiigures Adding
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

//

builder.Services.Configure<IdentityOptions>(options =>
{
    //add ýdentity options
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    // Lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
    //sýgn in options
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddSignalR();
builder.Services.ConfigureApplicationCookie(options =>
{
    //login road
    options.LoginPath = "/Account/login";
    options.LogoutPath = "/Account/logout";
    options.AccessDeniedPath = "/Account/Accessdenied";
    //cookie setting
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".datadb.Cookie",
        SameSite = SameSiteMode.Strict
    };

});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "600351898568-ionbk5uvugnqq2lj6rfoh7semctn0n5m.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.SignInScheme = IdentityConstants.ExternalScheme;


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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//routing Process
app.MapControllerRoute(
    name: "Account",
    pattern: "/Login/",
    defaults: new { controller = "Account", action = "Login" }
     );
app.MapControllerRoute(
    name: "AccountRegister",
    pattern: "/Register/",
    defaults: new { controller = "Account", action = "SignUp" }
     );
app.MapControllerRoute(
    name: "AccountConfirm",
    pattern: "/ConfirmMail/",
    defaults: new { controller = "Account", action = "ConfirmMail" }
     );
app.MapControllerRoute(
    name: "AccountForgot",
    pattern: "/ForgotPassword/",
    defaults: new { controller = "Account", action = "ForgotPassword" }
     );
app.MapControllerRoute(
    name: "ResetPassword",
    pattern: "/RePassword/",
    defaults: new { controller = "Account", action = "ResetPassword" }
     );
app.MapControllerRoute(
    name: "Admin",
    pattern: "/Admin/Dashboard/",
    defaults: new { controller = "Root", action = "index" }
     );
app.MapControllerRoute(
    name: "UserList",
    pattern: "/Admin/User/List/{id?}",
    defaults: new { controller = "Root", action = "UserList" }
     );
app.MapControllerRoute(
    name: "UserUpdate",
    pattern: "/Admin/User/{id?}",
    defaults: new { controller = "Root", action = "UserEdit" }
     );
app.MapControllerRoute(
    name: "RoleList",
    pattern: "/Admin/Role/List/{id?}",
    defaults: new { controller = "Root", action = "RoleList" }
     );
app.MapControllerRoute(
    name: "RoleUpdate",
    pattern: "/Admin/Role/Update/{id?}",
    defaults: new { controller = "Root", action = "RoleUpdate" }
     );
app.MapHub<DataHub>("/DataHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
