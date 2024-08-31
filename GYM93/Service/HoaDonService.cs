using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Service
{
    public class HoaDonService : IHoaDonService
    {   
        private readonly AppDbContext _appDbContext;
        private readonly IThanhVienService _thanhVienService;

        public HoaDonService(AppDbContext appDbContext, IThanhVienService thanhVienService)
        {
            _appDbContext = appDbContext;
            _thanhVienService = thanhVienService;
        }

        public async Task HoaDonCreate(HoaDon hoaDon)
        {
            hoaDon.NgayThanhToan = DateTime.Now;
            //Update membership
            ThanhVien thanhVien = await _thanhVienService.GetThanhVienById(hoaDon.ThanhVienId);
            if (hoaDon.NgayThanhToan.Year == thanhVien.NgayBatDau?.Year &&
               hoaDon.NgayThanhToan.Month == thanhVien.NgayBatDau?.Month)
            {
                thanhVien.NgayKetThuc = thanhVien.NgayKetThuc?.AddMonths(hoaDon.ThangDangKy);
            }
            else
            {
                thanhVien.NgayBatDau = hoaDon.NgayThanhToan;
                thanhVien.NgayKetThuc = hoaDon.NgayThanhToan.AddMonths(hoaDon.ThangDangKy);
            }    

            //
            _appDbContext.ThanhViens.Update(thanhVien);
            _appDbContext.Add(hoaDon);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<object> GetAllHoaDon()
        {
            var hoaDons = await _appDbContext.HoaDons
                .Include(h => h.ThanhVien) // Bao gồm bảng ThanhVien
                .OrderByDescending(h => h.NgayThanhToan)
                .Select(h => new
                {
                    h.HoaDonId,
                    TenThanhVien = h.ThanhVien.Ten,
                    h.NgayThanhToan,
                    h.ThangDangKy,
                    h.TongTien
                })
                .OrderByDescending(n => n.NgayThanhToan)
                .ToListAsync();

            return hoaDons;
        }

    }
}
