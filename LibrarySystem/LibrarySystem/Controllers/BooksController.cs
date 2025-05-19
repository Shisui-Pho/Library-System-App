using LibrarySystem.Data;
using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

public class BooksController : Controller
{
    private readonly IRepositoryWrapper _repository;
    public BooksController(IRepositoryWrapper repositoryWrapper)
    {
        _repository = repositoryWrapper;
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

        //Create the view model
        BooksDisplayViewModel model = new()
        {
            Books = books,
            PagingInfomation = paging
        };

        return View(model);
    }


    public IActionResult BookDetails(int id)
    {
        //this is the id passed
        Book book =_repository.Books.GetById(id);

        if(book == null)
        {
            //Return page not found
            return RedirectToAction("PageNotFound", "Home", new { message = "Book was not found. It may have been removed from the database." });
        }

        return View("BookDetails", book);
    }//BookDetails
}
