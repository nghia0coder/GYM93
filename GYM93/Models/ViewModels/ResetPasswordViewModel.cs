using System.ComponentModel.DataAnnotations;

namespace GYM93.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu Mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
