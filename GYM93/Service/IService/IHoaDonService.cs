using GYM93.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYM93.Service.IService
{
    public interface IHoaDonService
    {
        Task HoaDonCreate(HoaDon hoaDon);

        Task<object> GetAllHoaDon();
    }
}
