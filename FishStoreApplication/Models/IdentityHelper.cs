using Microsoft.AspNetCore.Identity;

namespace FishStoreApplication.Models
{
#nullable disable
    public class IdentityHelper
    {
        public const string Admin = "Admin";

        public const string Customer = "Customer";

        public static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            RoleManager<IdentityRole> roleManager = provider.GetService<RoleManager<IdentityRole>>();

            foreach(string role in roles)
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);
                if (!doesRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task CreateDefaultUser(IServiceProvider provider, string role)
        {
            var userManager = provider.GetService<UserManager<IdentityUser>>();

            int numUsers = (await userManager.GetUsersInRoleAsync(role)).Count();
            if (numUsers == 0)
            {
                var defaultUser = new IdentityUser()
                {
                    Email = "administrator@service.com",
                    UserName = "Admin1"
                };
                await userManager.CreateAsync(defaultUser, "AdminPass@1");
                await userManager.AddToRoleAsync(defaultUser, role);
            }
        }
    }
}
