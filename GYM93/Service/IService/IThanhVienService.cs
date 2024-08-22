using GYM93.Models;

namespace GYM93.Service.IService
{
    public interface IThanhVienService
    {
        Task ThanhVienCreate(ThanhVien thanhVien);
        Task ThanhVienUpdate(ThanhVien thanhVien);
        Task ThanhVienDelete(int thanhVienId);
        Task<IEnumerable<ThanhVien>> ThanhVienGetAll();
        Task<ThanhVien> GetThanhVienById(int? thanhVienId);

        bool ThanhVienExists(int thanhVienId);
    }
}
