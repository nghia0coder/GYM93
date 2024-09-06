using Microsoft.AspNetCore.Identity;

namespace GYM93.Models
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string? HinhAnhTv { get; set; }
	}
}
