using GYM93.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace GYM93.Service
{
    public class UserService : IUserService
    {
        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Admin", "NhanVien" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if(!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _user = await UserManager.FindByEmailAsync("admin@email.com");

            if (_user == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                };
                string adminPassword = "admin";

                var createAdmin = await UserManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
