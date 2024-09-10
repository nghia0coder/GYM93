using GYM93.Models;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using GYM93.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace GYM93.Controllers
{
    public class AuthController : Controller
    {   
        private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginSuccess = await _authService.Login(model);
                if(loginSuccess)
                {
                    TempData["success"] = "Đăng Nhập Thành Công";
					return RedirectToAction("Index", "Home");
				}    
                else
                {
					TempData["error"] = "Đăng Nhập Thất Bại. Kiểm Tra Lại Tên Khoản Và Mật Khẩu";
					return View(model);
				}    
            }
            TempData["error"] = "Đăng Nhập Thất Bại. Kiểm Tra Lại Tên Khoản Và Mật Khẩu";
			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
		    var result = await _authService.LogOut();
            if(result)
                return RedirectToAction(nameof(HomeController.Index), "Home");
            return View(result);
		}
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            AppUser model = await _authService.GetProfile();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeAccounts()
        {
            try
            {
                return View(await _authService.GetAllAccountNhanVien());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreatAccountEmpl()
        {
           
            if (_authService.GetAllAccountNhanVien().Result.Count >= SD.maxEmployeeAccounts)
            {
                // Hiển thị thông báo hoặc chuyển hướng về trang khác với thông báo
                TempData["error"] = "Số lượng tài khoản nhân viên đã đạt giới hạn. Không thể tạo thêm tài khoản mới.";
                return RedirectToAction("EmployeeAccounts"); // Redirect về trang danh sách hoặc trang khác
            }
            ViewData["username"] = await _authService.GenerateUserName();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProfileDetail(string userId)
        {
            return View(await _authService.GetProfileDetail(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatAccountEmpl(ProfileViewModel profileViewModel)
        {
            try
            {
                var isSuccess = await _authService.CreateAccountEployee(profileViewModel);
                if (isSuccess)
                {
                    TempData["success"] = "Tạo Tài Khoản Nhân Viên Thành Công";
                    return RedirectToAction("EmployeeAccounts");
                }    
                   
            }
            catch { }
            return View(profileViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string userId)
        {
            var account = await _authService.GetUserById(userId);
            return View(account);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var isSuccess = await _authService.ChangPassword(model);
            if(isSuccess)
            {
                TempData["success"] = "Đổi Mật Khẩu Thành Công";
                return RedirectToAction(nameof(Profile));
            }
            return View(model);
        }


            [HttpGet]
        public async Task<IActionResult> PersonalProfile(string userId)
        {
            var account = await _authService.GetUserById(userId);
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(AppUser model)
        {
            try
            {
                var isSuccess = await _authService.EditProfile(model);
                if (isSuccess)
                {
                    TempData["success"] = "Chỉnh Thông Tin Tài Khoản Nhân Viên Thành Công";
                    return RedirectToAction("EmployeeAccounts");
                }   
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var account = await _authService.GetUserById(userId);
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(AppUser model)
        {
            try
            {
                var isSuccess = await _authService.DeleteProfile(model);
                if (isSuccess)
                {
                    TempData["success"] = "Xóa Tài Khoản Nhân Viên Thành Công";
                    return RedirectToAction("EmployeeAccounts");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var isSuccess = await _authService.ForgotPassword(model);
            if(!isSuccess)
            {
                TempData["error"] = "Email Không Hợp Lệ";
                return RedirectToAction(nameof(Login));
            }
            TempData["success"] = "Hãy Kiểm Tra Email Của Bạn";
            return RedirectToAction(nameof(Login));

        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSucces = await _authService.ResetPassword(model);
            if (!isSucces)
            {
                TempData["success"] = "Đặt Lại Mật Khẩu Thất Bại";
                return View(model);
            }
            TempData["success"] = "Đặt Lại Mật Khẩu Thành Công";
            return RedirectToAction(nameof(Login));
        }
    }
}
