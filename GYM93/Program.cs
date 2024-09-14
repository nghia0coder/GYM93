using GYM93.Data;
using GYM93.Models;
using GYM93.Service;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Abstractions;
using GYM93.Utilities;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
//Azure Blob Storage
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetSection("AzureBlobStorage:BlobStorageConnectionStrings").Value));

//Add Identity Service
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
//
builder.Services.AddScoped<IThanhVienService, ThanhVienService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var urlHelperFactory = x.GetRequiredService<IUrlHelperFactory>();
    return urlHelperFactory.GetUrlHelper(new ActionContext(actionContext, new RouteData(), new ActionDescriptor()));
});
#region IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;


});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(5); // Thời gian hết hạn cookie, ở đây là 5 tiếng
    options.SlidingExpiration = true; // Cho phép gia hạn thời gian khi có hoạt động từ người dùng
    options.Cookie.IsEssential = true; // Cookie được coi là thiết yếu cho hoạt động của ứng dụng

    // Để logout ngay lập tức khi hết hạn
    options.Events.OnValidatePrincipal = context =>
    {
        var now = DateTimeOffset.UtcNow;
        if (context.Properties.ExpiresUtc.HasValue && context.Properties.ExpiresUtc.Value < now)
        {
            context.RejectPrincipal(); // Đánh dấu người dùng là không hợp lệ
            context.ShouldRenew = true; // Gia hạn lại cookie
        }
        return Task.CompletedTask;
    };
});


// Cấu hình đường dẫn mặc định cho trang đăng nhập
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Auth/Login"; // Đường dẫn đến trang đăng nhập
	options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi bị từ chối quyền truy cập
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

SD.Initialize(app.Services.GetRequiredService<IHttpContextAccessor>(), app.Services.GetService<IConfiguration>());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//using(var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var configuration = services.GetRequiredService<IConfiguration>();

//    await RoleInitializer.InitializeRoles(services, configuration);
//}    

app.Run();
