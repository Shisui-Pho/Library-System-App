using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
[Authorize]
public class DashboardController : Controller
{
    [Authorize(Roles ="Administrator")]
    public IActionResult AdminDashboard()
    {
        return View();
    }
    [Authorize(Roles ="Staff")]
    public IActionResult StaffDashboard()
    {
        return View();
    }//StaffDashboard
    public IActionResult CustomerDashboard()
    {
        return View();
    }//CustomerDashboard
}//class
