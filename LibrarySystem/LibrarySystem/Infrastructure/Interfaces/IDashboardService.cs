using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IDashboardService
{
    AdminStaffDashboardViewModel GetDashboardDetailsModel(ClaimsPrincipal user);
}
