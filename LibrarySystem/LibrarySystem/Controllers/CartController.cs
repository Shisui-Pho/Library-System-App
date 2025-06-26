using LibrarySystem.Infrastructure;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        this._cartService = cartService;
    }
    [HttpPost]
    public IActionResult AddToCart(CartItemViewModel model)
    {
        var (success, totalCartItems) = _cartService.AddToCart(HttpContext, model);
        return Json(new 
        { 
            success = success,
            cartCount = totalCartItems 
        });
    }
    [HttpGet]
    public IActionResult Index()
    {
        var cart = _cartService.GetCart(HttpContext);
        return View(cart);
    }//Index
}//class