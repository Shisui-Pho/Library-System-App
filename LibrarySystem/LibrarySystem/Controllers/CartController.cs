using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICheckoutService _checkoutService;

    //I am going to remove this context later, it's just for testing purposes
    private readonly AppDBContext _context;
    public CartController(ICartService cartService, ICheckoutService checkoutService, AppDBContext context)
    {
        this._cartService = cartService;
        this._checkoutService = checkoutService;
        //I am goinf to remove this context later, it's just for testing purposes
        this._context = context;
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
        var cart = _cartService.GetCart(HttpContext);
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
    //This page will need authorisation
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        var model = await _checkoutService.PrepareForCheckoutAsync(HttpContext);
        if (model == null)
        {
            return RedirectToAction("Index");
        }
        return View(model);
    }//Checkout
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        model.Cart = _cartService.GetCart(HttpContext);
        var(success, orderID) = await _checkoutService.CheckoutAsync(HttpContext, model);
        if (!success)
        {
            ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
            //Repopulate the other fields
            await _checkoutService.PopulateCheckoutViewModel(model, HttpContext);
            return View(model);
        }
        //If we reach here, it means the checkout was successful
        //Clear the cart after successful checkout
        _cartService.ClearCart(HttpContext);
        return RedirectToAction("OrderDetails", "Order", new { orderId = orderID});
    }
    [Authorize]
    public IActionResult Confirmation()
    {
        return View(default);
    }
}//CartController