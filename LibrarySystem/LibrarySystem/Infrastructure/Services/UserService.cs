using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Services;
public class UserService : IUserService
{
    //Contants
    private const string DEFAULT_ROLE = "Customer";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(UserManager<ApplicationUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }//
    public bool IsLoggedIn(ClaimsPrincipal user)
    {
        return user?.Identity != null && user.Identity.IsAuthenticated;
    }
    public string GetUserId(ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApplicationUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal user)
    {
        var id = GetUserId(user);

        if (id == null)
            return null;
        //Get the user
        return await _userManager.FindByIdAsync(id);
    }//GetCurrentLoggedInUserAsync

    public async Task<bool> LogInUser(LogInViewModel model)
    {
        //Find the user by their username/email
        var user = await _userManager.FindByEmailAsync(model.Email);

        //if the user was found
        if (user != null)
        {
            //log-out existing sign_ins
            await _signInManager.SignOutAsync();

            var results = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            return results.Succeeded;
        }

        return false;
    }//LogInUser

    public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
    {
        var errors = new List<IdentityError>();
        //Validate phone number if it was provided.
        if (model.ContactNumber != null)
        {
            bool isValidNumber = CustomValidations.IsValidSANumber(model.ContactNumber, out string message, out string normlised);
            if (!isValidNumber)
            {
                errors.Add(new() { Code = nameof(model.ContactNumber), Description = message });
            }
            //Replace phone number with the new normilised number
            model.ContactNumber = normlised;
        }
        //Verify passwords
        if (model.Password != model.ConfirmPassword)
        {
            errors.Add(new() { Code = nameof(model.ConfirmPassword), Description = "Passwords are not matching" });
            model.ConfirmPassword = "";//Reset the passwords
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
                errors.Add(new() { Code = "", Description = error.Description });
        }

        //Here the user was created
        //-Add to a role
        if (!await _roleManager.RoleExistsAsync(DEFAULT_ROLE))
        {
            results = await _roleManager.CreateAsync(new IdentityRole(DEFAULT_ROLE));
            if (!results.Succeeded)
            {
                errors.Add(new() { Code = "", Description = "Encountered an error while creating a profile. Please reach out the page admins for assistance." });
                //Remove the user
                await _userManager.DeleteAsync(user);
            }
        }
        var success = errors.Count == 0;
        if (success)
        {
            //Here role exists
            await _userManager.AddToRoleAsync(user, DEFAULT_ROLE);

            //sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            return IdentityResult.Success;
        }
        //return (success, errors);
        return IdentityResult.Failed([.. errors]);

    }//RegisterUser
    public async Task LogOutUser()
    {
        await _signInManager.SignOutAsync();
    }
}//class