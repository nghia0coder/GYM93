using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Service
{
    public class ThanhVienService : IThanhVienService
    {   
        private readonly AppDbContext _appDbContext;

        public ThanhVienService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ThanhVien> GetThanhVienById(int? thanhVienId)
        {
            if (thanhVienId == 0)
                throw new ArgumentException("Invalud ThanhVienID. Please Try Again");
            ThanhVien? thanhVien = await _appDbContext.ThanhViens.FindAsync(thanhVienId);
            return thanhVien ?? throw new ArgumentException("Thanh Vien Not Found");
                
        }

        public async Task ThanhVienCreate(ThanhVien thanhVien)
        {
            thanhVien.NgayThamGia = DateTime.Now;
            _appDbContext.Add(thanhVien);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task ThanhVienDelete(int thanhVienId)
        {
            var thanhVien = await _appDbContext.ThanhViens.FindAsync(thanhVienId);
            if (thanhVien != null)
            {
                _appDbContext.ThanhViens.Remove(thanhVien);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ThanhVien>> ThanhVienGetAll()
        {
            return await _appDbContext.ThanhViens.ToListAsync() ?? throw new ArgumentException("Thanh Vien Not Found");
        }

        public async Task ThanhVienUpdate(ThanhVien thanhVien)
        {
            try
            {
                _appDbContext.Update(thanhVien);
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThanhVienExists(thanhVien.ThanhVienId))
                {
                    throw new ArgumentException("Thanh Vien Not Found");
                }
                else
                {
                    throw;
                }
            }
        }

        public bool ThanhVienExists(int id)
        {
            return _appDbContext.ThanhViens.Any(e => e.ThanhVienId == id);
        }

     
    }
}
