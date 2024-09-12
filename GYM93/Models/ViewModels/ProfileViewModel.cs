using System.ComponentModel.DataAnnotations;

namespace GYM93.Models.ViewModels
{
	public class ProfileViewModel
	{
		public string UserName {  get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty ;
        [Display(Name = "Email Address", Prompt = "No Email")]
        public string Email { get; set; } = string.Empty ;
		public string PhoneNumber { get; set; } = string.Empty;
		public string HinhAnh { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập Lại Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
        public IFormFile Image { get; set; }
	}
}
