using BookShop.Constants;
using Microsoft.AspNetCore.Identity;
namespace BookShop.Areas.Identity.Data
{
    public class DbSeeder
    {


        
        public static async Task SeedDefaultDate(IServiceProvider serviceProvider)
        {
            // Initializing custom roles 
            var usrMgr = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleMgr = serviceProvider.GetService<RoleManager<IdentityRole>>();
            //Ading some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };
            var userInDb = await usrMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await usrMgr.CreateAsync(admin, "Admin@123");
                await usrMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }



        }
    }
}
