using GYM93.Models.ViewModels;

namespace GYM93.Service.IService
{
    public interface IThongKeService
    {
        Task<List<MonthlyRevenue>> GetMonthlyRevenues();

        Task<List<MonthlyRegistrations>> GetMonthlyRegistrations();
        Task<List<MontlyCompareRevenue>> GetMonthlyRevenueGrowth();
    }
}
