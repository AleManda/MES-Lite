using Microsoft.AspNetCore.Identity;

namespace MES_Lite.Web.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Supervisor", "Operatore", "QA" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        public static async Task SeedAdminAsync(UserManager<IdentityUser> userManager)
        {
            string adminEmail = "admin@meslite.local";
            string adminPassword = "Admin123!"; // potrai cambiarla dopo

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

    }
}
