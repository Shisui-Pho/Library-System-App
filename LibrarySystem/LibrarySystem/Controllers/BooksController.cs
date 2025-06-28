using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

public class BooksController : Controller
{
    private readonly IRepositoryWrapper _repository;
    private readonly ICartService _cartService;

    public BooksController(IRepositoryWrapper repositoryWrapper, ICartService cartService)
    {
        _repository = repositoryWrapper;
        _cartService = cartService;
    }
    //default page size
    private const int PAGE_SIZE = 16;
    public IActionResult List(int page = 1)
    {
        PagingInfomation paging = new()
        {
            CurrentPageNumber = page,
            NumberOfItemsPerPage = PAGE_SIZE,
            TotalNumberOfItems = _repository.Books.Count() //For now count here
        };

        QueryOptions<Book> queryOptions = new()
        {
            OrderBy = b => b.BookTitle,
            OrderByDirection = "asc", 
            PagingInfomation = paging
        };

        IEnumerable<Book> books = _repository.Books.GetWithOptions(queryOptions);
        var cartItems = _cartService.GetCart(HttpContext)
                                    .CartItems.Select(x => (x.BookID, x.Quantity))
                                    .ToList();
        //Create the view model
        BooksDisplayViewModel model = new(cartItems)
        {
            Books = books,
            PagingInfomation = paging
        };

        return View(model);
    }

    public IActionResult BookDetails(int id)
    {
        //this is the id passed
        var book = _repository.Books.GetBookWithAuthors(id);

        if(book == null)
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "Book was not found. It may have been removed from the database." });
        }

        return View("BookDetails", book);
    }//BookDetails
    [HttpPost]
    public IActionResult AddToCart(CartItemViewModel model)
    {
        //Use session based interactions
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? [];
        var item = model.CartItem;
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

        //Pass the current number of items
        TempData["NumberOfItemsInCart"] = cart.Count;

        //Redirect to the calling page
        return Redirect($"{model.PageWhereItemWasAdded}#book-{item.BookID}");
    }//AddToCart
}
