using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
namespace LibrarySystem.Infrastructure.Services;
public class BooksService : IBooksService
{
    private readonly IRepositoryWrapper _repo;
    public BooksService(IRepositoryWrapper repository)
    {
        this._repo = repository;
    }
    public IEnumerable<Book> GetBooks()
    {
        return _repo.Books.GetAllBooksWithAuthors();
    }//GetBooks

    public IEnumerable<Book> GetBooks(FilteringOptions filterOptions)
    {
        var options = new QueryOptions<Book>();

        //-TODO : Add a database script/StoreProcedure to do all the querying and table joins
        Expression<Func<Book, bool>> whereClause =
            p => p.BookTitle.Contains(filterOptions.SearchTerm ?? "") ||
                 p.Description.Contains(filterOptions.SearchTerm ?? "") ||
                 p.Genre.Equals(filterOptions.Genre ?? "");
        options.Where = whereClause;
        options.PagingInfomation = new PagingInfomation { CurrentPageNumber = filterOptions.Page, NumberOfItemsPerPage = 10};
        var books = _repo.Books.GetAllBooksWithAuthors(options);
        return books;
    }//GetBooks

    public IEnumerable<Book> GetBooks(AdvanceSearchViewModel searchParameters)
    {
        var words = searchParameters.Keywords.Split(',');
        //searchParameters.
        //searchParameters.
        Expression<Func<Book, bool>> whereClause =
            p => ContainsKeyWord(words, p) || p.BookTitle.Contains(searchParameters.Title) 
                || p.
    }//GetBooks
    private static bool ContainsKeyWord(string[] keyWords, Book book)
    {
        foreach (string word in keyWords)
            if (book.BookTitle.Contains(word) || book.Description.Contains(word))
                return true;
        return false;
    }//ContainsKeyWord
    public IEnumerable<string> GetGenres()
    {
        throw new NotImplementedException();
    }
    private static Expression<Func<T, object>> GetSortByClause<T>(string sortBy, out string orderByDirection)
        where T : class
    {
        orderByDirection = "asc"; //default ordering
        if (sortBy.EndsWith("_dsc") || sortBy.EndsWith("_asc") || sortBy.EndsWith("_desc"))
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
}//class
