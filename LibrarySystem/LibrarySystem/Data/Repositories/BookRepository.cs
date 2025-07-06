using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories;

public class BookRepository(AppDBContext dbContex)
             : BaseRepository<Book>(dbContex), IBookRepository
{
    public IEnumerable<Book> GetAllBooksWithAuthors(QueryOptions<Book> options = null)
    {
        if (options == null)
            return _dbContext.Books.Include(b => b.Authors);

        //Apply options
        return base.ApplyOptions(options).Include(b => b.Authors);
    }

    public Book GetBookWithAuthors(int id)
    {
        //Get the first book
        return _dbContext.Books.Where(b => b.Id == id).
                Include(b => b.Authors).
                FirstOrDefault();
    }//GetBookWithAuthors 
}//class