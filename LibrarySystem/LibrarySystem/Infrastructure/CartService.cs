using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.Infrastructure;
public class CartService : ICartService
{
    private readonly IUserService _userService;
    private readonly IRepositoryWrapper _repo;
    public CartService(IUserService userService, IRepositoryWrapper repository)
    {
        this._userService = userService;
        this._repo = repository;
    }
    public (bool success, int total) AddToCart(HttpContext context, CartItemViewModel item)
    {
        //-Here if the user is authenticated, we will save the cart info to the database,
        //  otherwise we use session session based interaction

        int totalCartItems = _userService.IsLoggedIn(context.User) ?
            AddToCartViaDatabase(item) : AddToCartViaSession(item, context);

        return (true, totalCartItems);
    }//class
    public CartViewModel GetCart(HttpContext context)
    {
        //Retrieve all cart items
        var cart = new CartViewModel();
        if (_userService.IsLoggedIn(context.User))
        {
            //Get it from database
            cart.CartItems = GetCartFromDB();
        }
        else
        {
            var lst = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
            //Populate the navigational property
            cart.CartItems = AddBookAndDetails(lst);
        }
        return cart;
    }
    #region Helpers
    private int AddToCartViaSession(CartItemViewModel cartInfo, HttpContext context)
    {
        //-Here if the user is authenticated, we will save the cart info to the database,
        //  otherwise we use session session based interaction
        List<CartItem> cart = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];

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
        context.Session.SetObjectAsJson("Cart", cart);
        return cart.Count;
    }
    private IEnumerable<CartItem> AddBookAndDetails(IEnumerable<CartItem> cartItems)
    {
        return cartItems.ApplyBookDetails(b => _repo.Books.GetBookWithAuthors(b.BookID));
    }//AddBookAndDetails
    [Authorize]
    private int AddToCartViaDatabase(CartItemViewModel cartInfo)
    {
        //For now
        throw new NotImplementedException();
    }
    [Authorize]
    private List<CartItem> GetCartFromDB()
    {
        throw new NotImplementedException();
    }
    #endregion Helpers
}//class