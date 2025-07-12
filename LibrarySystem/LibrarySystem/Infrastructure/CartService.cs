using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
    public int CountCartItems(HttpContext context)
    {
        int count = 0;
        if (_userService.IsLoggedIn(context.User))
        {
            var userid = _userService.GetUserId(context.User);
            count = _repo.Carts.FindByCondition(x => x.UserID == userid).Count();
        }
        else
        {
            count = context.Session.GetObjectFromJson<List<CartItem>>("Cart")?.Count ?? 0;
        }
        return count;
    }//CountCartItems
    public (bool success, int total) UpdateQuantity(HttpContext context,int bookId, int quantity)
    {
        if(quantity == 0)
        {
            //Remove the item from the 
            return RemoveFromCart(context, bookId);
        }
        bool sucess = false;
        //Update the quantity
        if (_userService.IsLoggedIn(context.User))
        {
            try
            {
                var userid = _userService.GetUserId(context.User);

                var item = _repo.Carts.GetCartOfUser(userid).FirstOrDefault();

                if(item != null)
                {
                    item.Quantity = quantity;
                    _repo.Carts.Update(item);
                    _repo.SaveChanges();
                    sucess = true;
                }
            }
            catch (DbException)
            {
                //Swallow exception
                sucess = false;
            }
        }
        else
        {
            //Use session
            var cart = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
            var index = cart.FindIndex(v => v.BookID == bookId);
            if(index >= 0)
            {
                cart[index].Quantity = quantity;
                context.Session.SetObjectAsJson("Cart", cart);
                sucess = true;
            }
        }
        return (sucess, CountCartItems(context));
    }//UpdateQuantity
    public (bool success, int total) AddToCart(HttpContext context, CartItemViewModel item)
    {
        //-Here if the user is authenticated, we will save the cart info to the database,
        //  otherwise we use session session based interaction

        int totalCartItems = _userService.IsLoggedIn(context.User) ?
            AddToCartViaDatabase(item, context) : AddToCartViaSession(item, context);
        
        //Update the session variable for the total
        if(totalCartItems >= 0 )
        {
            context.Session.SetInt32("cartCount", totalCartItems);
        }

        //-Negative value means something went wrong
        return (totalCartItems >= 0, totalCartItems);
    }//class
    public CartViewModel GetCart(HttpContext context)
    {
        //Retrieve all cart items
        var cart = new CartViewModel();
        if (_userService.IsLoggedIn(context.User))
        {
            //Get it from database
            var cartList = _repo.Carts.GetCartOfUser(_userService.GetUserId(context.User));
            cart.CartItems = cartList ?? [];
        }
        else
        {
            var lst = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
            //Populate the navigational property
            cart.CartItems = AddBookAndDetails(lst);
        }
        return cart;
    }
    public (bool success, int total) RemoveFromCart(HttpContext context, int bookID)
    {
        int totalItems = -1;
        if (_userService.IsLoggedIn(context.User))
        {
            var userid = _userService.GetUserId(context.User);
            try
            {
                var items = _repo.Carts.FindByCondition(cart => cart.UserID == userid && cart.BookID == bookID);
                if (items.Any())
                {
                    //-Here it should not be more than one item
                    //-We expect there to be only 1, just that the quantity may be more than one
                    if (items.Count() > 1)
                        throw new ArgumentException("Cart cannot contain more than one instance of a book");
                    var cart = items.FirstOrDefault();
                    _repo.Carts.Delete(cart);
                }
            }
            catch (DbException)
            {
                //Log somewhere
                return (false, -1);
            }
        }//end if
        else
        {
            var items = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
            int index = items.FindIndex(i => i.BookID == bookID);
            if(index >= 0)
            {
                items.RemoveAt(index);
                totalItems = items.Count;
                context.Session.SetObjectAsJson("Cart", items);
            }
        }

        //Update the session variable for the total
        if (totalItems >= 0)
        {
            context.Session.SetInt32("cartCount", totalItems);
        }

        //-Negative value means something went wrong
        return (totalItems >= 0, totalItems);
    }
    public void ClearCart(HttpContext context)
    {
        if(_userService.IsLoggedIn(context.User))
        {
            //Clear the cart from the database
            var userid = _userService.GetUserId(context.User);
            var items = _repo.Carts.FindByCondition(cart => cart.UserID == userid);
            if (items.Any())
            {
                try
                {
                    _repo.Carts.DeleteRange(items);
                    _repo.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //For now I swallow the exception
                    return;
                }
            }
        }
        else
        {
            //Clear the session
            context.Session.Remove("Cart");
        }
    }//ClearCart
    #region Helpers
    private static int AddToCartViaSession(CartItemViewModel cartInfo, HttpContext context)
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
    private int AddToCartViaDatabase(CartItemViewModel cartInfo, HttpContext context)
    {
        int count = -1;
        var userid = _userService.GetUserId(context.User);
        if (userid == null)
        {
            return -1;// sign that it was not added or cannot be addded
        }
        //Assign the user id to the cart item
        cartInfo.CartItem.UserID = userid;
        try
        {
            var item = _repo.Carts.FindByCondition(cart => cart.UserID == userid && cart.BookID == cartInfo.CartItem.BookID)
                                  .FirstOrDefault();
            if (item != null)
            {
                //Update the quantity
                item.Quantity++;
                _repo.Carts.Update(item);
            }
            else
            {
                //Create a new instance
                _repo.Carts.Create(cartInfo.CartItem);
            }
            _repo.SaveChanges();
            count = _repo.Carts.FindByCondition(cart => cart.UserID == userid && cart.BookID == cartInfo.CartItem.BookID)
                               .Count();
        }
        catch(DbUpdateException)
        {
            //Log somewhere in the application or something
            return count;
        }
        return count;
    }
    #endregion Helpers
}//class