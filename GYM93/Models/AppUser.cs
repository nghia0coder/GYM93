using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GYM93.Models
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string HinhAnhTv { get; set; }

        [Display(Name = "Hình Ảnh Thành Viên")]
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
