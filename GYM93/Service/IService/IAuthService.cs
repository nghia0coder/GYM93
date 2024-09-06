using GYM93.Models.ViewModels;

namespace GYM93.Service.IService
{
    public interface IAuthService
    {
        Task<bool> Login(LoginViewModel loginViewModel);

        Task<bool> LogOut();
       
        Task<ProfileViewModel> GetProfile();    
    }
}
