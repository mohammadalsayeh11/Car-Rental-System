using CentalMvc.Models;
using Microsoft.AspNetCore.Identity;

namespace CentalMvc.Data;

public static class DbSeeder
{
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // 1) Roles
        foreach (var role in new[] { AdminRole, CustomerRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // 2) Admin user
        const string adminEmail = "admin@cental.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "Site Admin",
                PhoneNumber = "+962790000000"
            };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, AdminRole);
        }

        // 3) Demo customer
        const string custEmail = "john@example.com";
        if (await userManager.FindByEmailAsync(custEmail) == null)
        {
            var cust = new ApplicationUser
            {
                UserName = custEmail,
                Email = custEmail,
                EmailConfirmed = true,
                FullName = "John Customer",
                PhoneNumber = "+962791111111"
            };
            var result = await userManager.CreateAsync(cust, "John@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(cust, CustomerRole);
        }
    }
}
