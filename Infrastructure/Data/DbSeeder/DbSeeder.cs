using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.DbSeeder
{
    public class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed Roles
            var roles = new[] { "Admin", "Manager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed SuperAdmin
            string AdminEmail = "abdofathy883@gmail.com";
            string password = "Aa123#";

            var existingUser = await userManager.FindByEmailAsync(AdminEmail);
            if (existingUser == null)
            {
                var superAdmin = new AppUser
                {
                    FirstName = "Abdo",
                    LastName = "Fathy",
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdmin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "Admin");
                }
                else
                {
                    throw new Exception($"Failed to create SuperAdmin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
