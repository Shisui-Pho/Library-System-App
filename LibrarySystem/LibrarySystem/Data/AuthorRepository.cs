using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class AuthorRepository(AppDBContext context) : BaseRepository<Author>(context), IAuthorRepository
{
    public IEnumerable<Author> GetAllAuthorsWithBooks()
    {
        return _dbContext.Authors.Include(auth => auth.Books);
    }

    public Author GetAuthorWithBooks(int id)
    {
       return _dbContext.Authors.Where(auth => auth.Id == id)
            .Include(auth => auth.Books)
            .FirstOrDefault();
    }
}//class
