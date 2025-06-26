using LibrarySystem.Infrastructure;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
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

    //This will be used by the AJAX method to update the cart list of
    //- the side bar cart
    [HttpGet]
    public IActionResult GetSideCartHtml()
    {
        // Render the SideCartViewComponent
        return new ViewComponentResult()
        {
            ViewComponentName = "SideCart"
        };
    }//GetSideCartHtml
    [HttpPost] 
    public IActionResult RemvoveFromCart(int bookid)
    {
        _ = _cartService.RemoveFromCart(HttpContext, bookid);
        var cart  = _cartService.GetCart(HttpContext);
        return View(cart);
    }//RemvoveFromCart
    [HttpPost]
    public IActionResult RemoveFromSideCart(int bookId)
    {
        var (sucess, totalItems) = _cartService.RemoveFromCart(HttpContext, bookId);
        return Json(new
        {
            success = sucess,
            totalItems = totalItems
        });
    }
}//class