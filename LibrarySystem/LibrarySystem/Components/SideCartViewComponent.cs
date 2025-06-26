using LibrarySystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Components;
public class SideCartViewComponent : ViewComponent
{
    private readonly ICartService _cartService;

    public SideCartViewComponent(ICartService cartService)
    {
        this._cartService = cartService;
    }
    public IViewComponentResult Invoke()
    {
        var cart = _cartService.GetCart(HttpContext);
        return View(cart);
    }
}//class
