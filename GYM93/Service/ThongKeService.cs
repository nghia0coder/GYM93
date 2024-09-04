using GYM93.Data;
using GYM93.Models.ViewModels;
using GYM93.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Service
{
    public class ThongKeService : IThongKeService
    {   
        private readonly AppDbContext _appDbContext;

        public ThongKeService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<MonthlyRegistrations>> GetMonthlyRegistrations()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Lấy số lượng thành viên đăng ký mới theo tháng từ tháng 1 đến tháng hiện tại
            var monthlyRegistrations = await _appDbContext.ThanhViens
                .Where(tv => tv.NgayBatDau.HasValue && tv.NgayBatDau.Value.Year == currentYear)
                .GroupBy(tv => tv.NgayBatDau.Value.Month)
                .Select(g => new MonthlyRegistrations
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Tạo danh sách các tháng từ 1 đến tháng hiện tại
            var monthlyData = Enumerable.Range(1, currentMonth)
                .Select(m => new MonthlyRegistrations
                {
                    Month = m,
                    Count = monthlyRegistrations.FirstOrDefault(r => r.Month == m)?.Count ?? 0
                })
                .ToList();

            return monthlyData;
        }

        public async Task<List<MontlyCompareRevenue>> GetMonthlyRevenueGrowth()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Lấy doanh thu theo tháng từ tháng 1 đến tháng hiện tại
            var monthlyRevenues = await _appDbContext.HoaDons
                .Where(hd => hd.NgayThanhToan.Year == currentYear)
                .GroupBy(hd => hd.NgayThanhToan.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(hd => hd.TongTien)
                })
                .OrderBy(m => m.Month)
                .ToListAsync();

            // Tính sự thay đổi doanh thu giữa các tháng
            var revenueChanges = new List<MontlyCompareRevenue>();
            decimal previousRevenue = 0;

            foreach (var monthRevenue in monthlyRevenues)
            {
                if (monthRevenue.Month == 1)
                {
                    revenueChanges.Add(new MontlyCompareRevenue
                    {
                        Month = monthRevenue.Month,
                        RevenueChange = 0 // Tháng đầu tiên không có sự thay đổi
                    });
                }
                else
                {
                    revenueChanges.Add(new MontlyCompareRevenue
                    {
                        Month = monthRevenue.Month,
                        RevenueChange = monthRevenue.TotalRevenue - previousRevenue
                    });
                }

                previousRevenue = monthRevenue.TotalRevenue;
            }

            return revenueChanges;
        }

        public async Task<List<MonthlyRevenue>> GetMonthlyRevenues()
        {
            var currentYear = DateTime.Now.Year;

            // Tạo danh sách 12 tháng với doanh thu mặc định là 0
            var monthlyRevenues = Enumerable.Range(1, 12)
                .Select(m => new MonthlyRevenue
                {
                    Month = m,
                    TotalRevenue = 0
                })
                .ToList();

            // Lấy doanh thu thực sự từ cơ sở dữ liệu
            var actualMonthlyRevenues = await _appDbContext.HoaDons
                .Where(h => h.NgayThanhToan.Year == currentYear)
                .GroupBy(h => h.NgayThanhToan.Month)
                .Select(g => new MonthlyRevenue
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(h => h.TongTien)
                })
                .ToListAsync();

            // Kết hợp doanh thu thực với danh sách mặc định
            foreach (var actualRevenue in actualMonthlyRevenues)
            {
                var monthData = monthlyRevenues.First(m => m.Month == actualRevenue.Month);
                monthData.TotalRevenue = actualRevenue.TotalRevenue;
            }

            return monthlyRevenues;
        }

        public async Task<PaymentStatus> GetPaymentStatus()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Tính toán ngày đầu tiên của tháng hiện tại
            var firstDayOfCurrentMonth = new DateTime(currentYear, currentMonth, 1);

            // Đếm số lượng thành viên đã thanh toán trong tháng hiện tại
            var paidCount = await _appDbContext.ThanhViens
                .Where(tv => tv.NgayKetThuc.HasValue &&
                             tv.NgayKetThuc.Value >= firstDayOfCurrentMonth)
                .Select(tv => tv.ThanhVienId)
                .Distinct()
                .CountAsync();

            // Đếm tổng số lượng thành viên
            var totalCount = await _appDbContext.ThanhViens.CountAsync();

            // Số lượng thành viên chưa thanh toán là tổng số trừ đi số thành viên đã thanh toán
            var unpaidCount = totalCount - paidCount;

            // Tạo đối tượng PaymentStatus để trả về
            var result = new PaymentStatus
            {
                PaidCount = paidCount,
                UnpaidCount = unpaidCount
            };

            return result;
        }
        public async Task<decimal> GetTotalEarning()
        {
            var currentYear = DateTime.Now.Year;

            var firstDayOfCurrentYear = new DateTime(currentYear, 1, 1);

            var today = DateTime.Now;

            var totalEarning = await _appDbContext.HoaDons
                                    .Where(n => n.NgayThanhToan >= firstDayOfCurrentYear && n.NgayThanhToan <= today)
                                    .SumAsync(hd => hd.TongTien);

            return totalEarning;
        }

        public async Task<decimal> GetTotalEarningCurrentMonth()
        {
            var currentMonth = DateTime.Now.Month;

            var totalEarning = await _appDbContext.HoaDons
                                    .Where(n => n.NgayThanhToan.Month == currentMonth)
                                    .SumAsync(hd => hd.TongTien);

            return totalEarning;
        }

        public async Task<int> GetTotalMembers()
        {
            var totalMembers = await _appDbContext.ThanhViens.CountAsync();

            return totalMembers;
        }

        public async Task<int> GetNewMemberCurrentMonth()
        {
            var currentMonth = DateTime.Now.Month;

            var totalNewMemerCurrentMotn = await _appDbContext.ThanhViens
                                .Where(n => n.NgayThamGia.HasValue && n.NgayThamGia.Value.Month == currentMonth)
                                .CountAsync();

            return  totalNewMemerCurrentMotn;
        }
    }
}
