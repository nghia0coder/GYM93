using GYM93.Controllers;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GYM93.Service
{
	public class AuthService : IAuthService
	{	
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
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

	
		public async Task LogOut()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
