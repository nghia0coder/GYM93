using System.Security.Claims;

namespace GYM93.Utilities
{
    public static class SD
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public const string Admin = "Admin";
        public const string NhanVien = "NhanVien";
        public const string AdminPassword = "AdminPassword";
        public const int maxEmployeeAccounts = 5;

        public static string GetCurrentUserId()
        {
            if (_httpContextAccessor?.HttpContext?.User != null)
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return null;
        }
     
    }
    
}
