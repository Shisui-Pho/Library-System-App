using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

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
    public IActionResult AuthorsList(int page = 1, string search = "", string sort = "full_name_asc")
    {
        var paging = new PagingInfomation()
        {
            CurrentPageNumber = page,
            NumberOfItemsPerPage = PAGE_SIZE - 1
        };
        int len = (sort.Length - sort.LastIndexOf('_'));
        //sort = $"FullName_{sort[^len..]}";
        var options = new QueryOptions<Author>                                                                       
        {
            OrderBy = GetSortByClause<Author>(sort, out string orderByDirection),
            OrderByDirection = orderByDirection, 
            PagingInfomation = paging, 
            Where = a => string.IsNullOrEmpty(search) || a.FullName.ToLower().Contains(search.ToLower())
        };

        var authors = _repository.Authors.GetWithOptions(options);
        if (authors == null || !authors.Any())
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "No authors were found in the database." });
        }
        var model = new AuthorsDisplayViewModel
        {
            Authors = authors,
            PagingInformation = options.PagingInfomation
        };
        return View(model);
    }//AuthorsList
    public IActionResult AdvanceSearch(AdvanceSearchViewModel model)
    {
        return View(model);
    }
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
    private static Expression<Func<T, object>> GetSortByClause<T>(string sortBy, out string orderByDirection)
        where T : class
    {
        orderByDirection = "asc"; //default ordering
        if(sortBy.EndsWith("_dsc") || sortBy.EndsWith("_asc") || sortBy.EndsWith("_desc"))
        {
            orderByDirection = sortBy[^3..];
            sortBy = sortBy.Replace("_dsc", "").Replace("_asc", "").Replace("_desc", "");
        }

        //Split and join properties in camel case
        sortBy = sortBy.Split('_').Apply(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).Join("");

        //Get order by expression
        Expression<Func<T, object>> orderby = p => EF.Property<T>(p, sortBy);
        return orderby;
    }//GetSortByClause
    private List<(int bookId, int quantity)> GetCartSummary()
    {
        var cartItems = _cartService.GetCart(HttpContext)
                                    .CartItems.Select(x => (x.BookID, x.Quantity))
                                    .ToList();
        return cartItems;
    }//GetCartSummary
}//class
