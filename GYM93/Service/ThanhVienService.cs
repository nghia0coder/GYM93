using Azure.Storage.Blobs;
using GYM93.Data;
using GYM93.Models;
using GYM93.Service.IService;
using GYM93.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Service
{
    public class ThanhVienService : IThanhVienService
    {
        private readonly AppDbContext _appDbContext;
        private readonly BlobServiceClient _blobServiceClient;

        public ThanhVienService(AppDbContext appDbContext, BlobServiceClient blobServiceClient)
        {
            _appDbContext = appDbContext;
            _blobServiceClient = blobServiceClient;
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

                // Get a reference to the container
                var containerClient = _blobServiceClient.GetBlobContainerClient(SD.ContainerName);

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the image to Azure Blob Storage
                using (var stream = thanhVien.Image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Set the HinhAnhTv to the blob URL
                thanhVien.HinhAnhTv = blobClient.Uri.ToString();
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
                    var containerClient = _blobServiceClient.GetBlobContainerClient(SD.ContainerName);
                    var blobName = Path.GetFileName(new Uri(thanhVien.HinhAnhTv).LocalPath);
                    var blobClient = containerClient.GetBlobClient(blobName);
                    await blobClient.DeleteIfExistsAsync();
                }
                _appDbContext.ThanhViens.Remove(thanhVien);

                _appDbContext.SaveChanges();

            }

        }

        public async Task<IEnumerable<ThanhVien>> ThanhVienGetAll()
        {
            return await _appDbContext.ThanhViens.ToListAsync() ?? throw new ArgumentException("Thanh Vien Not Found");
        }

        public async Task ThanhVienUpdate(int id, ThanhVien thanhVien)
        {
            try
            {
                ThanhVien member = await _appDbContext.ThanhViens.FindAsync(id);

                if (member == null)
                {
                    throw new ArgumentException("Thanh Vien Not Found");
                }

                if (thanhVien.Image != null)
                {
                    // Delete old image from Blob Storage if it exists
                    if (!string.IsNullOrEmpty(member.HinhAnhTv))
                    {
                        var containerClient1 = _blobServiceClient.GetBlobContainerClient(SD.ContainerName);
                        var oldBlobName = Path.GetFileName(new Uri(member.HinhAnhTv).LocalPath);
                        var oldBlobClient = containerClient1.GetBlobClient(oldBlobName);
                        await oldBlobClient.DeleteIfExistsAsync();
                    }

                    // Upload new image to Blob Storage
                    string fileName = member.ThanhVienId + Path.GetExtension(thanhVien.Image.FileName);
                    var containerClient = _blobServiceClient.GetBlobContainerClient(SD.ContainerName);
                    var blobClient = containerClient.GetBlobClient(fileName);

                    using (var stream = thanhVien.Image.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    member.HinhAnhTv = blobClient.Uri.ToString();
                }

                // Update other properties
                member.Ten = thanhVien.Ten;
                member.Sđt = thanhVien.Sđt;
                member.GioiTinh = thanhVien.GioiTinh;
                member.BienSoXe = thanhVien.BienSoXe;
                member.HoVaTenDem = thanhVien.HoVaTenDem; 
                // ... update other properties as needed ...

                _appDbContext.Entry(member).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThanhVienExists(id))
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
