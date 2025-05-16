using System.Threading.Tasks;
using EcommerceBackend.DTOs;

namespace EcommerceBackend.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}