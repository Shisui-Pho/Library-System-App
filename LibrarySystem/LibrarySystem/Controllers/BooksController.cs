using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LibrarySystem.Controllers;

public class BooksController : Controller
{
    private readonly ICartService _cartService;
    private readonly IBooksService _bookService;
    private readonly AppDBContext context;

    public BooksController(ICartService cartService, IBooksService booksService, AppDBContext context)
    {
        _cartService = cartService;
        _bookService = booksService;
        this.context = context;
    }
    //default page size
    private const int PAGE_SIZE = 16;
    public async Task<IActionResult> BooksList(int page = 1, string genre = null, string top = null, string pricerange=null, string searchterm=null)
    { 
        var filter = new BookFilteringOptions()
        {
            Genre = genre,
            PriceRange = pricerange,
            SearchTerm = searchterm,
            Top = top, 
            PageSize = PAGE_SIZE,
            Page = page
        };

        var books = await _bookService.GetBooks(filter);

        //Create the view model
        BooksDisplayViewModel model = new(GetCartSummary())
        {
            Books = books,
            FilteringOptions = filter, 
            Genres = _bookService.GetGenres()
        };

        return View(model);
    }
    public IActionResult BookDetails(int id)
    {
        //this is the id passed
        var book = _bookService.GetBookDto(id);

        if(book == null)
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "Book was not found. It may have been removed from the database." });
        }

        return View("BookDetails", book);
    }//BookDetails
    public IActionResult AuthorsList(int page = 1, string search = "", string sort = "full_name_asc")
    {
        var filter = new AuthorFilteringOptions()
        {
            Page = page,
            PageSize = PAGE_SIZE - 1,
            SearchTerm = search,
            PropertyName = GetSortProperty(sort, out string direction), 
            SortDirection = direction
        };

        var authors = _bookService.GetAuthors(filter);

        var model = new AuthorsDisplayViewModel
        {
            Authors = authors,
            PagingInformation = filter.Paging
        };
        return View(model);
    }//AuthorsList
    public IActionResult AdvanceSearch(AdvancedBookFilteringOptions model)
    {
        return View(model);
    }
    public IActionResult AuthorBooks(int id)
    {
        //Get the author
        var (author, books) = _bookService.GetAuthorWithBooks(id);
        if (author == null)
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "Author was not found. It may have been removed from the database." });
        }

        //Create the view model
        var model = new BookAuthorViewModel(GetCartSummary())
        {
            Author = author,
            Books = books
        };
        return View(model);
    } //class
    private static string GetSortProperty(string sortBy, out string orderByDirection)
    {
        orderByDirection = "asc"; //default ordering
        if (sortBy.EndsWith("_dsc") || sortBy.EndsWith("_asc") || sortBy.EndsWith("_desc"))
        {
            orderByDirection = sortBy[^3..];
            sortBy = sortBy.Replace("_dsc", "").Replace("_asc", "").Replace("_desc", "");
        }

        //Split and join properties in camel case
        sortBy = sortBy.Split('_').Apply(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).Join("");

        return sortBy;
    }//GetSortByClause

    private List<(int bookId, int quantity)> GetCartSummary()
    {
        var cartItems = _cartService.GetCart(HttpContext)
                                    .CartItems.Select(x => (x.BookID, x.Quantity))
                                    .ToList();
        return cartItems;
    }//GetCartSummary
    public IActionResult GoGenre()
    {
        //This method just servers for populating the genre table
        // in the database, it will be removed later once the database is populated
        var Books = context.Books.Include(b => b.Genre);

        foreach (var book in Books)
        {
            string genre = book.Genre;
            string[] genres = genre.Split(',');
            foreach(var g in genres)
            {
                var trimmedGenre = g.Trim();
                var existingGenre = context.Genres.Where(x=> x.Name == trimmedGenre).FirstOrDefault();

                if(existingGenre == null)
                {
                    //Create a new genre
                    var newGenre = new Genre()
                    {
                        Name = trimmedGenre, 
                        Description = "This is a genre created by the system. Please update it with a proper description."
                    };
                    context.Genres.Add(newGenre);
                    context.SaveChanges();//Save the changes to the database
                    existingGenre = newGenre;
                }

                //add the book to the genre
                existingGenre.Books ??= new List<Book>();
                existingGenre.Books.Add(book);
                //book.Genres
            }
        }

        return RedirectToAction("BooksList", new { genre = "all" });
    }//GoGenre
}//class