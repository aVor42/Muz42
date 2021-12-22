using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AudioShmaudio.Models;

namespace AudioShmaudio
{
    public class RoleInitializer
    {

        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminLogin = "admin";
            var adminPassword = "Admin111-";
            var adminName = "admin";
            var userName = "user";
            var moderatorName = "moderator";

            if (await roleManager.FindByNameAsync(adminName) == null)
                await roleManager.CreateAsync(new IdentityRole(adminName));

            if (await roleManager.FindByNameAsync(userName) == null)
                await roleManager.CreateAsync(new IdentityRole(userName));

            if (await roleManager.FindByNameAsync(moderatorName) == null)
                await roleManager.CreateAsync(new IdentityRole(moderatorName));

            if (userManager.Users.FirstOrDefault(user => user.Login == adminLogin) == null)
            {
                var admin = new User { Login = adminLogin, UserName = adminName };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, adminName);
            }

        }
    }
}
