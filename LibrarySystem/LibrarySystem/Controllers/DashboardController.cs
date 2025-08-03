using LibrarySystem.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashService;
    private const string STAFF_ADMIN_DASH = "Index";
    private const string CUSTOMER_DASH = "User";
    public DashboardController(IDashboardService dashService)
    {
        this._dashService = dashService;
    }

    [Authorize(Roles ="Administrator")]
    public async Task<IActionResult> AdminDashboard()
    {
        var model = await _dashService.GetDashboardDetailsModel();
        model.IsAdmin = true;
        return View(STAFF_ADMIN_DASH, model);
    }
    [Authorize(Roles ="Staff")]
    public async Task<IActionResult> StaffDashboard()
    {
        var model = await _dashService.GetDashboardDetailsModel();
        model.IsAdmin = false;
        return View(STAFF_ADMIN_DASH, model);
    }//StaffDashboard
    public IActionResult CustomerDashboard()
    {
        return View();
    }//CustomerDashboard
}//class
