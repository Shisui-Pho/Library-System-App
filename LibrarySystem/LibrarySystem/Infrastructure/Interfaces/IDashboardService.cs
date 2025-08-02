using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IDashboardService
{
    Task<AdminStaffDashboardViewModel?> GetDashboardDetailsModel();
}
