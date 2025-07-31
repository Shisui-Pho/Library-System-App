using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models.ViewModels;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Services;
public class DashboardService : IDashboardService
{
    private readonly IUserService _userService;
    private readonly IRepositoryWrapper _repo;

    public DashboardService(IRepositoryWrapper repo, IUserService userService)
    {
        this._userService = userService;
        this._repo  = repo;
    }
    #nullable enable
    public AdminStaffDashboardViewModel? GetDashboardDetailsModel(ClaimsPrincipal user)
    {
        if (_userService.IsLoggedIn(user))
            return null;

        return default;
    }//GetDashboardDetailsModel

    

}//class