using LibrarySystem.Models;
namespace LibrarySystem.Data.Repositories;
public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetAllBooksWithAuthors();
    Book GetBookWithAuthors(int id);
}
