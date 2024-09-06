using GYM93.Controllers;
using GYM93.Models;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GYM93.Service
{
	public class AuthService : IAuthService
	{	
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(UserManager<AppUser> userManager, 
							SignInManager<AppUser> signInManager,
							IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ProfileViewModel> GetProfile()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

			var model = new ProfileViewModel
			{
				UserName = user.UserName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				HinhAnh = user.HinhAnhTv
			};

			return model;
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
	}
}
