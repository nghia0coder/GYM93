using System.Security.Claims;

namespace GYM93.Utilities
{
    public static class SD
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static IConfiguration _configuration;
        public const string Admin = "Admin";
        public const string NhanVien = "NhanVien";
        public const string AdminPassword = "AdminPassword";
        public const int maxEmployeeAccounts = 5;
        public static string ContainerName {  get; set; }

        public static void Initialize(IHttpContextAccessor httpContextAccessor,
                                       IConfiguration configuration)
        {   
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            ContainerName = _configuration.GetSection("AzureBlobStorage:ContainerName").Value;
        }
      

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
