using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
namespace LibrarySystem.Controllers;

[Authorize(Roles ="Customer")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private const int ITEMS_PER_PAGE = 10;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public IActionResult Orders()
    {
        var options = new QueryOptions<Order>()
        {
            OrderBy = x => x.CreatedAt,
            OrderByDirection = "dsc",
            PagingInfomation = new()
            {
                CurrentPageNumber = 1,
                NumberOfItemsPerPage = ITEMS_PER_PAGE
            }
        };

        var orders = _orderService.GetOrders(HttpContext.User, options);

        var model = new OrdersDisplayViewModel()
        {
            Orders = orders,
            PagingInfomation = options.PagingInfomation
        };
        return View(model);
    }//Orders
    public IActionResult OrderDetails(int id)
    {
        var order = _orderService.GetOrderDetails(HttpContext.User, id);
        return View(order);
    }//OrderDetails
    [HttpPost]
    public IActionResult CancelOrder(int id)
    {
        var result = _orderService.CancelOrder(HttpContext.User, id);
        if (result)
        {
            TempData["SuccessMessage"] = "Order cancelled successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to cancel the order.";
        }
        return RedirectToAction("Orders");
    }//CancelOrder
}//class