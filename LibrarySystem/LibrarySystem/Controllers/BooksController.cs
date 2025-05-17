using LibrarySystem.Data;
using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

public class BooksController : Controller
{
    private readonly IRepositoryWrapper _repository;
    public BooksController(IRepositoryWrapper repositoryWrapper)
    {
        _repository = repositoryWrapper;
    }
    public IActionResult List()
    {
        //Load all the books for now
        QueryOptions<Book> queryOptions = new()
        {
            OrderBy = b => b.BookTitle,
            OrderByDirection = "asc"
        };

        IEnumerable<Book> books = _repository.Books.GetWithOptions(queryOptions); 
        return View(books);
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
