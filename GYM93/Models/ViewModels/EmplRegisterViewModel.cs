using System.ComponentModel.DataAnnotations;

namespace GYM93.Models.ViewModels
{
    public class EmplRegisterViewModel
    {
        [Required]   
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
    }
}
