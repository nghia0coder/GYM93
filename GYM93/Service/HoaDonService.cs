using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;
using GYM93.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GYM93.Service
{
    public class HoaDonService : IHoaDonService
    {   
        private readonly AppDbContext _appDbContext;
        private readonly IThanhVienService _thanhVienService;
        private readonly UserManager<AppUser> _userManager;

        public HoaDonService(AppDbContext appDbContext, IThanhVienService thanhVienService, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _thanhVienService = thanhVienService;
            _userManager = userManager;
        }

        public async Task HoaDonCreate(HoaDon hoaDon)
        {
            hoaDon.NgayThanhToan = DateTime.Now;

            // Tính số ngày đăng ký dựa trên số tháng đăng ký (1 tháng = 30 ngày)
            hoaDon.SoNgayDangKy = hoaDon.ThangDangKy * 30;

            // Lấy thông tin thành viên từ cơ sở dữ liệu
            ThanhVien thanhVien = await _thanhVienService.GetThanhVienById(hoaDon.ThanhVienId);

            // Tính toán logic dựa trên ngày thanh toán
            if (thanhVien != null)
            {
                var soNgayConLai = (thanhVien.NgayKetThuc - DateTime.Now)?.Days ?? 0;

                if (soNgayConLai > 0)
                {
                    // Thành viên vẫn còn hạn, gia hạn thêm số ngày đăng ký
                    thanhVien.NgayKetThuc = thanhVien.NgayKetThuc?.AddDays(hoaDon.SoNgayDangKy);
                }
                else
                {
                    // Thành viên đã hết hạn, thiết lập lại ngày bắt đầu và kết thúc mới
                    thanhVien.NgayBatDau = hoaDon.NgayThanhToan;
                    thanhVien.NgayKetThuc = hoaDon.NgayThanhToan.AddDays(hoaDon.SoNgayDangKy);
                }

                // Cập nhật số tiền đã đóng
                thanhVien.SoTienDaDong += hoaDon.TongTien;
                
                _appDbContext.ThanhViens.Update(thanhVien);
            }

            var userId = SD.GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId);
            hoaDon.TenNguoiThanhToan = user.FullName;
            // Thêm hóa đơn vào cơ sở dữ liệu
            _appDbContext.Add(hoaDon);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<object> GetAllHoaDon()
        {
            var hoaDons = await _appDbContext.HoaDons
                .Include(h => h.ThanhVien)
                .OrderByDescending(h => h.NgayThanhToan)
                .Select(h => new
                {
                    h.HoaDonId,
                    TenThanhVien = h.ThanhVien.Ten,
                    h.NgayThanhToan,
                    h.SoNgayDangKy,
                    h.TongTien,
                    h.TenNguoiThanhToan
                })
                .OrderByDescending(n => n.HoaDonId)
                .ToListAsync();

            return hoaDons;
        }

        public async Task<bool> VisitorCreate(HoaDon hoaDon)
        {
            hoaDon.NgayThanhToan = DateTime.Now;
            //Update membership
            ThanhVien thanhVien = await _thanhVienService.GetThanhVienById(hoaDon.ThanhVienId);
            if (hoaDon.NgayThanhToan.Year == thanhVien.NgayBatDau?.Year &&
               hoaDon.NgayThanhToan.Month == thanhVien.NgayBatDau?.Month)
            {
                thanhVien.NgayKetThuc = thanhVien.NgayKetThuc?.AddDays(hoaDon.SoNgayDangKy);
            }
            else
            {
                thanhVien.NgayBatDau = hoaDon.NgayThanhToan;
                thanhVien.NgayKetThuc = hoaDon.NgayThanhToan.AddDays(hoaDon.SoNgayDangKy);
            }

            thanhVien.SoTienDaDong += hoaDon.TongTien;
            _appDbContext.ThanhViens.Update(thanhVien);
            _appDbContext.Add(hoaDon);
            await _appDbContext.SaveChangesAsync();
            return  true;   
        }
    }
}
