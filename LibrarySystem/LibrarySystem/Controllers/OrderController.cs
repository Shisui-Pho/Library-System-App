using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
[Authorize]
public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult OrderDetails(int orderId)
    {
        return View();
    }//OrderDetails
}//class
