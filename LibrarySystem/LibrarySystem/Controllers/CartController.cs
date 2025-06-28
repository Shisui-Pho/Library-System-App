using LibrarySystem.Infrastructure;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public CartController(ICartService cartService, IUserService userService)
    {
        this._cartService = cartService;
        this._userService = userService;
    }
    [HttpPost]
    public IActionResult AddToCart(CartItemViewModel model)
    {
        //Check if the user is logged in or not
        if (_userService.IsLoggedIn(HttpContext.User))
        {
            //Assign the user id to the model
            model.CartItem.UserID = _userService.GetUserId(HttpContext.User);
        }
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
    }//RemoveFromSideCart
    public IActionResult UpdateQuantity(int bookId, int quantity)
    {
        //Max quantity is 10
        var (success, totalItems) = (false, _cartService.CountCartItems(HttpContext));

        //Make sure the range is within the range
        if (quantity >= 0 && quantity <= 10)
        {
            //If the quantity is 0, it means we need to remove the item from the cart,
            //- otherwise we update the quantity

            //-This function call will handle remove and update function
            (success, totalItems) = _cartService.UpdateQuantity(HttpContext, bookId, quantity);
        }
        return Json(new
        {
            success = success,
            totalItems = totalItems
        });
    }//UpdateQuantity
}//class