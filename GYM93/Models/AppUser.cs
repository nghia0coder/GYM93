using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GYM93.Models
{
	public class AppUser : IdentityUser
    {
        [Display(Name = "Họ Và Tên", Prompt = "Trống")]   
        
		public string FullName { get; set; } = string.Empty;
		public string HinhAnhTv { get; set; }

        [Display(Name = "Hình Ảnh Thành Viên")]
        [NotMapped]
        public IFormFile Image { get; set; }
        [NotMapped]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
    }
}
