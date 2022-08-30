using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PakenhamPhysiotherapy.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("booking", policy =>
        policy.RequireRole("admin", "cust", "staff"));
    options.AddPolicy("Approve&Reject", policy =>
        policy.RequireRole("admin", "staff"));
    options.AddPolicy("JobsIndex", policy =>
        policy.RequireRole("admin", "staff"));
}).AddOpenIdConnect(options =>
{
    options.SignInScheme = "Cookies";
    options.Authority = "-your-identity-provider-";
    options.RequireHttpsMetadata = true;
    options.ClientId = "-your-clientid-";
    options.ClientSecret = "-your-client-secret-from-user-secrets-or-keyvault";
    options.ResponseType = "code";
    options.UsePkce = true;
    options.Scope.Add("profile");
    options.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
