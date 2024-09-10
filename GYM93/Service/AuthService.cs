using Azure.Core;
using GYM93.Controllers;
using GYM93.Data;
using GYM93.Models;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using GYM93.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Security.Policy;
using IEmailSender = GYM93.Service.IService.IEmailSender;

namespace GYM93.Service
{
	public class AuthService : IAuthService
	{	
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;
		public AuthService(UserManager<AppUser> userManager, 
							SignInManager<AppUser> signInManager,
							IHttpContextAccessor httpContextAccessor,
							RoleManager<IdentityRole> roleManager,
							IConfiguration configuration,
                            IEmailSender emailSender,
                            IUrlHelper urlHelper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
			_roleManager = roleManager;
			_configuration = configuration;
            _emailSender = emailSender;
            _urlHelper = urlHelper;
		}

        public async Task<bool> ChangPassword(ChangePasswordViewModel model)
        {
            
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return false;
            }

            // Thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Đăng nhập lại sau khi đổi mật khẩu thành công
                await _signInManager.RefreshSignInAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> CreateAccountEployee(ProfileViewModel model)
        {
			var user = new AppUser
			{
				UserName = model.UserName,
				FullName = model.FullName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				HinhAnhTv = model.HinhAnh
			};


			var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Bạn có thể gán role "NhanVien" cho user sau khi tạo thành công
                await _userManager.AddToRoleAsync(user, SD.NhanVien.ToString());
                if (model.Image != null)
                {
                    string fileName = user.Id + Path.GetExtension(model.Image.FileName);
                    string filePath = @"wwwroot/memberImages/" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        model.Image.CopyTo(fileStream);
                    }


                    user.HinhAnhTv = "memberImages/" + fileName;
                }
                else
                {
                    user.HinhAnhTv = "https://placehold.co/600x400";
                }
                await _userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }    
            }    

            // Trả về false nếu tạo tài khoản không thành công
            return false;
        }

        public async Task<bool> DeleteProfile(AppUser appUser)
        {
            var user = await _userManager.FindByNameAsync(appUser.UserName);
            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(user.HinhAnhTv))
            {
                var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var oldFilePathDirectory = Path.Combine(webRootPath, user.HinhAnhTv.Replace("/", "\\"));

                if (File.Exists(oldFilePathDirectory))
                {
                    File.Delete(oldFilePathDirectory);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
               
            
                return true;   
            }
            return false;
        }

        public async Task<bool> EditProfile(AppUser appUser)
        {
            var user = await _userManager.FindByNameAsync(appUser.UserName);
            if (user == null)
            {
                return false;
            }
           
            user.FullName = appUser.FullName;
            user.Email = appUser.Email;
            user.PhoneNumber = appUser.PhoneNumber;
            
            if (appUser.Image != null)
            {
                if (!string.IsNullOrEmpty(appUser.HinhAnhTv))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), appUser.HinhAnhTv);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                }
                string fileName = appUser.Id + Path.GetExtension(appUser.Image.FileName);
                string filePath = @"wwwroot\memberImages\" + fileName;
                var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                {
                    appUser.Image.CopyTo(fileStream);
                }
                user.HinhAnhTv = "memberImages/" + fileName;
            }

            if (appUser.Password != null)
            {
                // Xóa mật khẩu cũ nếu người dùng đã có mật khẩu
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    var removeResult = await _userManager.RemovePasswordAsync(user);
                    if (!removeResult.Succeeded)
                    {
                        return false;
                    }
                }

                // Thêm mật khẩu mới
                var addPasswordResult = await _userManager.AddPasswordAsync(user, appUser.Password);
                if (!addPasswordResult.Succeeded)
                {
                    return false;
                }
            }
          



            var result = await _userManager.UpdateAsync(user);
           
            if (result.Succeeded)
            {   
                return true;
            }
			return false;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordViewModel model)
        {
           

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return false;
            }    
            bool isInRole = await _userManager.IsInRoleAsync(user, SD.Admin);
            if (!isInRole)
            {
                // Không tiết lộ thông tin người dùng không tồn tại
                return false;
            }

            // Tạo mã token reset mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = _urlHelper.Action("ResetPassword", "Auth", new { token, email = model.Email }, "https");

            string emailBody = $@"
                <div style='font-family: Arial, sans-serif; font-size: 16px; color: #333;'>
                    <h2 style='color: #2c3e50;'>Đặt Lại Mật Khẩu</h2>
                    <p>Xin chào,</p>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu. Vui lòng nhấp vào liên kết bên dưới để đặt lại mật khẩu của bạn:</p>
                    <p style='text-align: center;'>
                        <a href='{resetLink}' style='padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px; font-size: 18px;'>Đặt Lại Mật Khẩu</a>
                    </p>
                    <p>Nếu bạn không yêu cầu đặt lại mật khẩu, bạn có thể bỏ qua email này.</p>
                    <p>Trân trọng,</p>
                
                </div>";


            // Gửi email
            await _emailSender.SendEmail(model.Email, "Đặt lại mật khẩu",
                 emailBody);

            return true;
        }

        public async Task<string> GenerateUserName()
        {
            var existingUsers = await _userManager.Users
							.Where(u => u.UserName.StartsWith("nhanvien"))
							.ToListAsync();

            // Tìm số thứ tự lớn nhất
            int maxIndex = 0;

            foreach (var user in existingUsers)
            {
                // Lấy phần số từ username (ví dụ: nhanvien2 -> 2)
                string numberPart = user.UserName.Substring("nhanvien".Length);

                if (int.TryParse(numberPart, out int currentIndex))
                {
                    // So sánh với số thứ tự cao nhất hiện có
                    if (currentIndex > maxIndex)
                    {
                        maxIndex = currentIndex;
                    }
                }
            }

            // Tạo username mới với số thứ tự lớn hơn 1 đơn vị nếu có nhân viên trước đó
            string newUserName;
            if (maxIndex == 0 && existingUsers.Count == 0)
            {
                // Nếu chưa có nhân viên nào, khởi tạo với nhanvien1
                newUserName = "nhanvien1";
            }
            else
            {
                // Tạo username mới với số thứ tự lớn hơn 1 đơn vị
                newUserName = "nhanvien" + (maxIndex + 1).ToString();
            }

			return newUserName;
        }

        public async Task<List<AppUser>> GetAllAccountNhanVien()
        {
            if(! await _roleManager.RoleExistsAsync(SD.NhanVien))
			{
                throw new Exception("Role không tồn tại.");

            }
			var usersInRole = await _userManager.GetUsersInRoleAsync(SD.NhanVien);

			return usersInRole.ToList();
        }

        public async Task<AppUser> GetProfile()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

			

			return user;
		}

        public async Task<ProfileViewModel> GetProfileDetail(string userId)
        {
			AppUser model = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

			var profile = new ProfileViewModel
			{
				UserName = model.UserName,
				FullName = model.FullName,
				PhoneNumber = model.PhoneNumber,
				Email = model.Email,
				HinhAnh = model.HinhAnhTv
			};

			return profile;
        }

        public async Task<AppUser> GetUserById(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
			return user;
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<bool> Login(LoginViewModel loginViewModel)
		{
			var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
			if (user != null)
			{
				var result = await _signInManager.PasswordSignInAsync(user.UserName ?? " ", loginViewModel.Password,false, lockoutOnFailure :false);
				if (result.Succeeded)
				{
					return true;	
				}
				return false;
			}
			return false;
		}

	
		public async Task<bool> LogOut()
		{
			await _signInManager.SignOutAsync();
			return true;
		}

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
