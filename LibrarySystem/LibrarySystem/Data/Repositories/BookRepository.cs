using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.DTO;
using LibrarySystem.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibrarySystem.Data.Repositories;
#nullable enable
public class BookRepository(AppDBContext dbContex)
             : BaseRepository<Book>(dbContex), IBookRepository
{
    public IEnumerable<Book> GetAllBooksWithDetails(QueryOptions<Book>? options = null)
    {
        IQueryable<Book> books;
        if (options == null)
            books = _dbContext.Books;
        else
            books = ApplyOptions(options);

        return books.Include(b => b.Authors).Include(b => b.BookInteractions);
    }//GetAllBooksWithDetails

    public Book? GetBookWithDetails(int id)
    {
        //Get the first book
        return _dbContext.Books.Where(b => b.Id == id)
                .Include(b => b.Authors)
                .Include(b => b.BookInteractions)
                .FirstOrDefault();
    }//GetBookWithDetails 
    public BookDto? GetBookDto(int id)
    {
        var idParam = new SqlParameter("@BookId", id);
        
        //Execute the stored procedure to get the book DTO
        var book = _dbContext.Set<BookDto>()
            .FromSqlRaw($"EXEC Proc_GetBookDtoValuesByBookId @BookId", idParam)
            .AsEnumerable()
            .FirstOrDefault();

        //Get the book with authors and interactions
        var bookExtraDetails = _dbContext.Set<Book>().Where(b => b.Id == id)
            .Include(b => b.Authors)
            .Include(b => b.BookInteractions)
            .Include(b => b.Genres)
            .AsNoTracking()
            .FirstOrDefault();

        //If the book is not found, return null
        if (bookExtraDetails != null && book != null)
        {
            book.Authors = bookExtraDetails.Authors;
            book.BookInteractions = bookExtraDetails.BookInteractions;
            book.Genres = bookExtraDetails.Genres;
        }

        return book;
    }//GetBookDto
    public async Task<IEnumerable<Book>> GetBooksByFilter(BookFilteringOptions options)
    {
        var top = new SqlParameter("@Top", (object?)options.Top ?? DBNull.Value);
        var genre = new SqlParameter("@Genre", (object?)options.Genre ?? DBNull.Value);
        var searchTerm = new SqlParameter("@SearchTerm", (object?)options.SearchTerm ?? DBNull.Value);
        var priceRange = new SqlParameter("@PriceRange", (object?)options.PriceRange ?? DBNull.Value);
        var page = new SqlParameter("@Page", options.Paging.CurrentPageNumber);
        var size = new SqlParameter("@PageSize", options.Paging.NumberOfItemsPerPage);

        //Define output parameter
        var totalCount = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        var books = await _dbContext.Books
            .FromSqlRaw("EXEC Proc_Normal_FilterBooks @Top, @Genre, @SearchTerm, @PriceRange, @Page, @PageSize, @TotalCount OUTPUT",
                        top, genre, searchTerm, priceRange, page, size, totalCount)
            .ToListAsync();

        options.Paging.TotalNumberOfItems = (int)totalCount.Value;
        return books;
    }//GetBooksByFilter

    public async Task<IEnumerable<string>> GetTopNGenres(int topN)
    {
        if (topN <= 0)
            topN = 5;//Default to 5 if not specified

        var topNParam = new SqlParameter("@TopN", topN);

        var topGenres = await _dbContext.Set<string>()
            .FromSqlRaw("EXEC Proc_GetTopNGenres @TopN", topNParam)
            .ToListAsync();

        return topGenres;
    }//GetTopNGenres
}//class