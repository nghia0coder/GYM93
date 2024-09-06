using System.ComponentModel.DataAnnotations;

namespace GYM93.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
	}
}
