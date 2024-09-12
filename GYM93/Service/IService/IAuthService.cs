using GYM93.Models;
using GYM93.Models.ViewModels;

namespace GYM93.Service.IService
{
    public interface IAuthService
    {
        Task<bool> Login(LoginViewModel loginViewModel);

        Task<bool> LogOut();

        Task<List<AppUser>> GetAllAccountNhanVien();
       
        Task<AppUser> GetProfile();

        Task<ProfileViewModel> GetProfileDetail(string userId);

        Task<AppUser> GetUserById(string userId);
        Task<AppUser> GetUserByUserName(string userName);

        Task<bool> EditProfile(AppUser appUser);

        Task<bool> DeleteProfile(AppUser appUser);

        Task<string> GenerateUserName();

        Task<bool> ChangPassword(ChangePasswordViewModel  model);

        Task<bool> CreateAccountEployee(ProfileViewModel model);

        Task<bool> ForgotPassword(ForgotPasswordViewModel model);

        Task<bool> ResetPassword(ResetPasswordViewModel model);

    }
}
