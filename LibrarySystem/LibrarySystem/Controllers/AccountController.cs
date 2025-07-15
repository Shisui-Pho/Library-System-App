using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
//[Authorize]
public class AccountController : Controller
{
    //Dependecy injections
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public AccountController(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }//Register

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUser(model);

            if (result.Succeeded)
            {
                //Merge session cart
                var cart = _cartService.GetCart(HttpContext);
                MergeSessionCartToDatabaseCart(cart.CartItems);
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
            }

            //Here add errors to model state and display them
            result.Errors.Apply(p => ModelState.AddModelError(p.Code, p.Description));
        }
        return View(model);
    }//Register

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }//Login

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LogInViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var isLoggedIn = await _userService.LogInUser(model);

            if (isLoggedIn)
            {
                var cart = _cartService.GetCart(HttpContext);
                MergeSessionCartToDatabaseCart(cart.CartItems);
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
            }
        }
        model.Password = "";
        ModelState.AddModelError("", "Invalid username or password");
        return View(model);
    }//Login

    private void MergeSessionCartToDatabaseCart(IEnumerable<CartItem> items)
    {
        //Update the number of items in the cart from db
        var totalCartItems = _cartService.CountCartItems(HttpContext);

        HttpContext.Session.SetInt32("cartCount", totalCartItems);
        if (_userService.IsLoggedIn(HttpContext.User))
        {
            //Add all the items to the dbcart
            foreach (var item in items)
            {
                //Now that the user has been logged in, the items will be added to the database
                var userid = _userService.GetUserId(User);
                item.UserID = userid;
                _cartService.AddToCart(HttpContext, new CartItemViewModel() { CartItem = item });
            }
        }
    }//MergeSessionCartToDatabaseCart
    public async Task<IActionResult> Logout()
    {
        await _userService.LogOutUser();
        return RedirectToAction("Index", "Home");
    }//SignOut
    public IActionResult AccessDenied()
    {
        return View();
    }//AccessDenied
}//class