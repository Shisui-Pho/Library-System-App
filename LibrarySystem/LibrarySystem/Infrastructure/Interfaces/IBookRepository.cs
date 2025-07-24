using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using LibrarySystem.Models.DTO;
using LibrarySystem.Models.ViewModels;
namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBookRepository : IBaseRepository<Book>
{
    IEnumerable<Book> GetAllBooksWithDetails(QueryOptions<Book> qeryOptions = null);
    Book GetBookWithDetails(int id);
    Task<IEnumerable<Book>> GetBooksByFilter(BookFilteringOptions options);
    BookDto GetBookDto(int id);
}
