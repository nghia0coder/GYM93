using GYM93.Models;
using GYM93.Utilities;
using Microsoft.AspNetCore.Identity;

namespace GYM93.RoleIInitializer
{
    public static class RoleInitializer
    {
        public static async Task InitializeRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roleNames = { SD.Admin, SD.NhanVien };

            IdentityResult roleResult;

            // Tạo các vai trò nếu chưa tồn tại
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tạo tài khoản Admin nếu chưa tồn tại
            var _user = await UserManager.FindByEmailAsync("admin@email.com");

            if (_user == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                };
                string adminPassword = configuration[SD.AdminPassword];

                var createAdmin = await UserManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, SD.Admin);
                }
                else
                {
                    Console.WriteLine("Error creating admin account:");
                    foreach (var error in createAdmin.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }
        }
    }
}
