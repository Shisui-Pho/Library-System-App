using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
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
    public IActionResult BooksList(int page = 1)
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

        IEnumerable<Book> books = _repository.Books.GetAllBooksWithAuthors(queryOptions);
        
        //Create the view model
        BooksDisplayViewModel model = new(GetCartSummary())
        {
            Books = books,
            PagingInfomation = paging
        };

        return View(model);
    }
    private List<(int bookId, int quantity)> GetCartSummary()
    {
        var cartItems = _cartService.GetCart(HttpContext)
                                    .CartItems.Select(x => (x.BookID, x.Quantity))
                                    .ToList();
        return cartItems;
    }//GetCartSummary
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
    public IActionResult AuthorsList()
    {
        var authors = _repository.Authors.FindAll();
        if (authors == null || !authors.Any())
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "No authors were found in the database." });
        }
        return View(authors);
    }//AuthorsList
    public IActionResult AuthorBooks(int id)
    {
        //Get the author
        var author = _repository.Authors.GetById(id);
        if (author == null)
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "Author was not found. It may have been removed from the database." });
        }
        //Get the books by the author
        var books = _repository.Books.FindByCondition(b => b.Authors.Any(a => a.Id == id))
                                      .OrderBy(b => b.BookTitle)
                                      .ToList();
        //Create the view model
        var model = new BookAuthorViewModel(GetCartSummary())
        {
            Author = author,
            Books = books
        };
        return View(model);
    } //class
}//class