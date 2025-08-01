using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models.Identity;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Services;
public class UserService : IUserService
{
    //Contants
    private const string DEFAULT_ROLE = "Customer";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContext;
    private readonly AppIdentityDBContext _identityDb;

    public UserService(UserManager<ApplicationUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       SignInManager<ApplicationUser> signInManager, 
                       IHttpContextAccessor context, 
                       AppIdentityDBContext identityDb)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _httpContext = context;
        _identityDb = identityDb;
    }//
    public bool IsLoggedIn()
    {
        var user = _httpContext.HttpContext?.User;
        return user?.Identity != null && user.Identity.IsAuthenticated;
    }
    public string GetUserId()
    {
        var user = _httpContext.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApplicationUser> GetCurrentLoggedInUserAsync()
    {
        var id = GetUserId();

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
            if (results.Succeeded)
            {
                //Add user to the "currently logged in users" table
                AddUserToLogInTable(user);
            }
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

            //Add user to the "currently logged in users" table
            AddUserToLogInTable(user);

            return IdentityResult.Success;
        }
        //return (success, errors);
        return IdentityResult.Failed([.. errors]);

    }//RegisterUser
    public async Task LogOutUser()
    {
        var user = await GetCurrentLoggedInUserAsync();
        await _signInManager.SignOutAsync();
        RemoveUserFromLogInTable(user);
    }
    private void AddUserToLogInTable(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentException("Phiwo, the claims principal hasn't been updated yet. FIX THIS!!!!!");

        var sessionId = _httpContext.HttpContext.Session.Id;
        var newLogIn = new LoggedInUser()
        {
            LastActivity = DateTime.UtcNow,
            LoginTime = DateTime.UtcNow,
            SessionId = sessionId,
            UserId = user.Id,
            IPAddress = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString()
        };

        _identityDb.loggedInUsers.Add(newLogIn);
        try
        {
            _identityDb.SaveChanges();
        }
        catch (Exception ex)
        {
            _ = ex;
            //TODO: Log into somewhere
            //_identityDb.Remove(newLogIn);
        }

    }//AddUserToLogInTable
    private void RemoveUserFromLogInTable(ApplicationUser user)
    {

        if (user == null)
            throw new ArgumentException("Phiwo, the claims principal hasn't been updated yet. FIX THIS!!!!!");

        var sessionId = _httpContext.HttpContext.Session.Id;

        var logIn = _identityDb.loggedInUsers.FirstOrDefault(x => x.UserId == user.Id && x.SessionId == sessionId);
        if(logIn == null)
        {
            //TODO: Handle cases where a session for the currently logged in user is not found
            return;
        }
        _identityDb.loggedInUsers.Remove(logIn);
        try
        {
            _identityDb.SaveChanges();
        }
        catch (Exception ex)
        {            
            //TODO: Log into somewhere
            _ = ex;
        }
        }//RemoveUserFromLogInTable
}//class