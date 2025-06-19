using LibrarySystem.Data;
using LibrarySystem.Infrastructure;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
public class CartController : Controller
{
    private readonly IRepositoryWrapper _repository;
    public CartController(RepositoryWrapper repo)
    {
        this._repository = repo;
    }
    [HttpPost]
    public IActionResult AddToCart(CartItemViewModel model)
    {
        //-Here if the user is authenticated, we will save the cart info to the database,
        //  otherwise we use session session based interaction

        int totalCartItems = User.Identity.IsAuthenticated ? 
            AddToCartViaDatabase(model) : AddToCartViaSession(model);

        return Json(new 
        { 
            success = true,
            cartCount = totalCartItems 
        });
    }
    private int AddToCartViaDatabase(CartItemViewModel cartInfo)
    {
        //For now
        throw new NotImplementedException();
    }
    private int AddToCartViaSession(CartItemViewModel cartInfo)
    {
        //-Here if the user is authenticated, we will save the cart info to the database,
        //  otherwise we use session session based interaction
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
        
        var item = cartInfo.CartItem;
        var existingItem = cart.FirstOrDefault(c => c.BookID == item.BookID);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;//increament the quantity of the item
        }
        else
        {
            //Add the item
            cart.Add(item);
        }

        //Add to the current user session objects
        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return cart.Count;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
