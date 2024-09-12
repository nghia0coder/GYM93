using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;
using Microsoft.AspNetCore.Mvc;
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
            thanhVien.SoTienDaDong = 0;
            _appDbContext.Add(thanhVien);
            await _appDbContext.SaveChangesAsync();
            if (thanhVien.Image != null)
            {
                string fileName = thanhVien.ThanhVienId + Path.GetExtension(thanhVien.Image.FileName);
                string filePath = @"wwwroot/memberImages/" + fileName;
                var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                {
                    thanhVien.Image.CopyTo(fileStream);
                }


                thanhVien.HinhAnhTv = "memberImages/" + fileName;
            }
            else
            {
                thanhVien.HinhAnhTv = "https://placehold.co/600x400";
            }

            _appDbContext.ThanhViens.Update(thanhVien);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task ThanhVienDelete(int thanhVienId)
        {
            var thanhVien = await _appDbContext.ThanhViens.FindAsync(thanhVienId);
            if (thanhVien != null)
            {

                if (!string.IsNullOrEmpty(thanhVien.HinhAnhTv))
                {
                    var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var oldFilePathDirectory = Path.Combine(webRootPath, thanhVien.HinhAnhTv.Replace("/", "\\"));

                    if (File.Exists(oldFilePathDirectory))
                    {
                        File.Delete(oldFilePathDirectory);
                    }
                }
                _appDbContext.ThanhViens.Remove(thanhVien);

                _appDbContext.SaveChanges();

            }

        }

        public async Task<IEnumerable<ThanhVien>> ThanhVienGetAll()
        {
            return await _appDbContext.ThanhViens.ToListAsync() ?? throw new ArgumentException("Thanh Vien Not Found");
        }

        public async Task ThanhVienUpdate(ThanhVien thanhVien)
        {
            try
            {
         
          
                if (thanhVien.Image != null)
                {
                    if (!string.IsNullOrEmpty(thanhVien.HinhAnhTv))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), thanhVien.HinhAnhTv);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                    }
                    string fileName = thanhVien.ThanhVienId + Path.GetExtension(thanhVien.Image.FileName);
                    string filePath = @"wwwroot\memberImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        thanhVien.Image.CopyTo(fileStream);
                    }
                    thanhVien.HinhAnhTv = "memberImages/" + fileName;
                }
                else
                {
                    thanhVien.HinhAnhTv = _appDbContext.ThanhViens.AsNoTracking()
                                    .Where(tv => tv.ThanhVienId == thanhVien.ThanhVienId)
                                    .Select(tv => tv.HinhAnhTv)
                                    .FirstOrDefault();
                }    

                _appDbContext.ThanhViens.Update(thanhVien);
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

        public JsonResult SearchThanhVien(string term)
        {
            var thanhviens = _appDbContext.ThanhViens
                .Where(n => n.Ten.Contains(term))
                .Select(n => new { n.Ten, n.ThanhVienId, n.HinhAnhTv })
                .ToList();

            return new JsonResult(thanhviens);
        }

        public async Task<JsonResult> GetMembersAsync(string searchQuery = "", // Chuỗi tìm kiếm
                    int pageNumber = 1, // Số trang hiện tại
                    int pageSize = 10, // Số lượng mục trên mỗi trang
                    string sortBy = "Tn", // Cột sắp xếp
                    bool sortAscending = true)
        {
            var query = _appDbContext.ThanhViens.AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(m => m.Ten.Contains(searchQuery) || m.Sđt.Contains(searchQuery));
            }

            // Sắp xếp
            switch (sortBy.ToLower())
            {
                case "ten":
                    query = sortAscending ? query.OrderBy(m => m.Ten) : query.OrderByDescending(m => m.Ten);
                    break;
                default:
                    query = sortAscending ? query.OrderByDescending(m => m.ThanhVienId) : query.OrderByDescending(m => m.Ten);
                    break;
            }

            // Phân trang
            var totalItems = await query.CountAsync();
            var members = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Trả về kết quả với thông tin phân trang
            var result = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Members = members
            };

            return new JsonResult(result);
        }

      
    }
}
