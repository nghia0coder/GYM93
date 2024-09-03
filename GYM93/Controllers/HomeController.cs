using GYM93.Models;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GYM93.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IThongKeService _thongKeService;

        public HomeController(ILogger<HomeController> logger, IThongKeService thongKeService)
        {
            _logger = logger;
            _thongKeService = thongKeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetRevenueChart()
        {
            var monthlyRevenues = await _thongKeService.GetMonthlyRevenues();

            // Tạo dữ liệu cho biểu đồ
            var labels = monthlyRevenues.Select(m => $"Tháng {m.Month}").ToArray();
            var data = monthlyRevenues.Select(m => m.TotalRevenue).ToArray();

            return Json(new { labels, data });
        }

        public async Task<IActionResult> GetMonthlyRegistrations()
        {
            var monthlyRegistrations = await _thongKeService.GetMonthlyRegistrations();


            return Json(monthlyRegistrations);
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlyRevenueGrowth()
        {
            var monthlyRevenueGrowth = await _thongKeService.GetMonthlyRevenueGrowth();
            return Json(monthlyRevenueGrowth);
        }

    }
}
