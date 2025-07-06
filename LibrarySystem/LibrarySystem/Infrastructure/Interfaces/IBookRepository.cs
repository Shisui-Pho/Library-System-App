using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetAllBooksWithAuthors(QueryOptions<Book> qeryOptions = null);
    Book GetBookWithAuthors(int id);
}
