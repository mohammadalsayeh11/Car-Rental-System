using CentalMvc.Data;
using CentalMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(4);
});

var app = builder.Build();

// ---------------------------------------------------------
// Database bootstrap:
// Recreate the database fresh so seed data is always correct.
// (Great for development / demos — no manual migration needed.)
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var svc = scope.ServiceProvider;
    var db = svc.GetRequiredService<AppDbContext>();

    // Force-drop the old database (even if connections are open) and rebuild it
    // clean. This guarantees no broken/old seed data remains.
    Console.WriteLine(">>> CENTAL: Rebuilding database (fresh seed)...");
    DbInitializer.ForceRecreate(db, builder.Configuration);
    await DbSeeder.SeedAsync(svc);
    Console.WriteLine(">>> CENTAL: Database ready. Admin = admin@cental.com / Admin@123");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
