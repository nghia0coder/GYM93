using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Mvc;

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
    }
}
