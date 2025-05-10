using LibrarySystem.Models;
namespace LibrarySystem.Data;
public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetAllBooksWithAuthors();
    Book GetBookWithAuthors(int id);
}
