using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories;

public class BookRepository(AppDBContext dbContex)
             : BaseRepository<Book>(dbContex), IBookRepository
{
    public IEnumerable<Book> GetAllBooksWithAuthors()
    {
        return _dbContext.Books.Include(b => b.Authors);
    }

    public Book GetBookWithAuthors(int id)
    {
        //Get the first book
        return _dbContext.Books.Where(b => b.Id == id).
                Include(b => b.Authors).
                FirstOrDefault();
    }//GetBookWithAuthors 
}//class