using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IDashboardService
{
    Task<AdminStaffDashboardViewModel?> GetDashboardDetailsModel();
}
