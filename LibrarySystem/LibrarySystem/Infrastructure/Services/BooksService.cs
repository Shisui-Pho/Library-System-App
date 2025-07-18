using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using LibrarySystem.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
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

    public async Task<IEnumerable<Book>> GetBooks(BookFilteringOptions filterOptions)
    {
        PagingInfomation paging = new()
        {
            CurrentPageNumber = filterOptions.Page,
            NumberOfItemsPerPage = filterOptions.PageSize 
        };
        filterOptions.Paging = paging;
        var books = await _repo.Books.GetBooksByFilter(filterOptions);
        return books;
    }//GetBooks

    public IEnumerable<Book> GetBooks(AdvancedBookFilteringOptions searchParameters)
    {
        throw new NotImplementedException("Advance search is not implemented yet.");
    }//GetBooks
    public IEnumerable<string> GetGenres()
    {
        AdvancedQueryOptions<Book, string> options = new()
        {
            Select = p => p.Genre, 
            SelectDistinct = true
        };

        //Select the genres
        var genres = _repo.Books.GetWithOptions(options);
        return genres;
    }
    public Book GetBook(int bookId)
    {
        return _repo.Books.GetBookWithAuthors(bookId);
    }
    public (Author author, IEnumerable<Book> books) GetAuthorWithBooks(int authorID)
    {
        var author = _repo.Authors.GetById(authorID);
        if (author == null)
            return (author, []);

        var options = new QueryOptions<Book>()
        {
            Where = b => b.Authors.Any(a => a.Id == authorID),
            OrderBy = b => b.BookTitle,
            OrderByDirection = "asc"
        };

        var authorBooks = _repo.Books.GetWithOptions(options);
        return (author, authorBooks);
    }//GetAuthorWithBooks
    public IEnumerable<Author> GetAuthors(AuthorFilteringOptions filteringOptions)
    {
        var paging = new PagingInfomation()
        {
            CurrentPageNumber = filteringOptions.Page,
            NumberOfItemsPerPage = filteringOptions.PageSize
        };

        var options = new QueryOptions<Author>
        {
            OrderBy = p => EF.Property<Author>(p, filteringOptions.PropertyName),
            OrderByDirection = filteringOptions.SortDirection,
            PagingInfomation = paging,
            Where = a => string.IsNullOrEmpty(filteringOptions.SearchTerm) || 
                         a.FullName.ToLower().Contains(filteringOptions.SearchTerm.ToLower())
        };

        var authors = _repo.Authors.GetWithOptions(options);

        filteringOptions.Paging = options.PagingInfomation;
        return authors;
    }//GetAuthors
    //private static Expression<Func<T, object>> GetSortByClause<T>(string sortBy, out string orderByDirection)
    //    where T : class
    //{
    //    orderByDirection = "asc"; //default ordering
    //    if (sortBy.EndsWith("_dsc") || sortBy.EndsWith("_asc") || sortBy.EndsWith("_desc"))
    //    {
    //        orderByDirection = sortBy[^3..];
    //        sortBy = sortBy.Replace("_dsc", "").Replace("_asc", "").Replace("_desc", "");
    //    }

    //    //Split and join properties in camel case
    //    sortBy = sortBy.Split('_').Apply(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).Join("");

    //    //Get order by expression
    //    Expression<Func<T, object>> orderby = p => EF.Property<T>(p, sortBy);
    //    return orderby;
    //}//GetSortByClause
}//class
