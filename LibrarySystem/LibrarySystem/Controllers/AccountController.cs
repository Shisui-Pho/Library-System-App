using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
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
    
    public AccountController(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
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
            //Verify passwords
            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords are not matching");
                model.ConfirmPassword = model.Password = "";//Reset the passwords
                return View(model);
            }

            //Register the user here
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                PhoneNumber = model.ContactNumber,
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
            
            //sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

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

                var results = await _signInManager.PasswordSignInAsync(user,model.Password, false, false);
                if(results.Succeeded)
                {
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