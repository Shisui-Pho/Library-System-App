using LibrarySystem.Models;
namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetAllBooksWithAuthors();
    Book GetBookWithAuthors(int id);
}
