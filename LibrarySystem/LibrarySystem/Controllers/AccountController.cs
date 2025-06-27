using LibrarySystem.Infrastructure;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
//[Authorize]
public class AccountController : Controller
{
    //Contants
    private const string DEFAULT_ROLE = "Customer";

    //Dependecy injections
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ICartService _cartService;
    
    public AccountController(SignInManager<ApplicationUser> signInManager, 
                            RoleManager<IdentityRole> roleManager, 
                            UserManager<ApplicationUser> userManager,
                            ICartService cartService)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
        _cartService = cartService;
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
            //Validate phone number if it was provided.
            if(model.ContactNumber != null)
            {
                bool isValidNumber = CustomValidations.IsValidSANumber(model.ContactNumber, out string message, out string normlised);
                if (!isValidNumber)
                {
                    ModelState.AddModelError(nameof(model.ContactNumber), message);
                    return View(model);
                }
                //Replace phone number with the new normilised number
                model.ContactNumber = normlised;
            }
            //Verify passwords
            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(model.ConfirmPassword), "Passwords are not matching");
                model.ConfirmPassword = "";//Reset the passwords
                return View(model);
            }

            //Register the user here
            ApplicationUser user = new()
            {
                Email = model.Email,
                PhoneNumber = model.ContactNumber ?? "",
                UserName = model.Email, //The username is the same as their email
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            //Register the user
            var results = await _userManager.CreateAsync(user, model.Password);

            if (!results.Succeeded)
            {
                foreach (var error in results.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            //Here the user was created
            //-Add to a role
            if(!await _roleManager.RoleExistsAsync(DEFAULT_ROLE))
            {
                results = await _roleManager.CreateAsync(new IdentityRole(DEFAULT_ROLE));
                if (!results.Succeeded)
                {
                    ModelState.AddModelError("", "Encountered an error while creating a profile. Please reach out the page admins for assistance.");
                    //Remove the user
                    await _userManager.DeleteAsync(user);
                    return View(model);
                }
            }

            //Here role exists
            await _userManager.AddToRoleAsync(user, DEFAULT_ROLE);

            //Get the user's cart before login(from session)
            var cart = _cartService.GetCart(HttpContext);

            //sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            //Merge carts
            MergeSessionCartToDatabaseCart(cart.CartItems);

            //redirect the user to their desired place
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
            
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
            //Find the user by their username/email
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            //if the user was found
            if(user != null)
            {
                //log-out existing sign_ins
                await _signInManager.SignOutAsync();
                //Get the carts ready for a merge
                var cart = _cartService.GetCart(HttpContext);
                var results = await _signInManager.PasswordSignInAsync(user,model.Password, false, false);
                if(results.Succeeded)
                {
                    //Merge carts
                    MergeSessionCartToDatabaseCart(cart.CartItems);
                    //Redirect user to the return url or home page
                    return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
                }
            }
            //Reset the password
            model.Password = "";
            ModelState.AddModelError("", "Invalid username or password");
        }
        return View(model);
    }//Login
    private void MergeSessionCartToDatabaseCart(IEnumerable<CartItem> items)
    {
        if(User.Identity.IsAuthenticated)
        {
            //Add all the items to the dbcart
            foreach (var item in items)
            {
                //Now that the user has been logged in, the items will be added to the database
                _cartService.AddToCart(HttpContext, new CartItemViewModel() { CartItem = item });
            }
        }
    }//MergeSessionCartToDatabaseCart
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }//SignOut
    public IActionResult AccessDenied()
    {
        return View();
    }//AccessDenied
}//class