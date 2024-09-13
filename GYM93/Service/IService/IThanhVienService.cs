using GYM93.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYM93.Service.IService
{
    public interface IThanhVienService
    {
        Task ThanhVienCreate(ThanhVien thanhVien);
        Task ThanhVienUpdate(int id,ThanhVien thanhVien);
        Task ThanhVienDelete(int thanhVienId);
        Task<IEnumerable<ThanhVien>> ThanhVienGetAll();
        Task<ThanhVien> GetThanhVienById(int? thanhVienId);

        Task<JsonResult> GetMembersAsync(string searchQuery, 
                              int pageNumber = 1,
                              int pageSize = 10,
                              string sortBy = "Tên",
                              bool sortAscending = true);

        JsonResult SearchThanhVien(string term);
        bool ThanhVienExists(int thanhVienId);
    }
}
