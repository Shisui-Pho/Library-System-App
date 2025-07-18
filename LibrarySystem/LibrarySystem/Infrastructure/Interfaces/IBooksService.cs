using LibrarySystem.Models;
using LibrarySystem.Models.DTO;
using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBooksService
{
    IEnumerable<Author> GetAuthors(AuthorFilteringOptions filteringOptions);
    (Author author, IEnumerable<Book> books) GetAuthorWithBooks(int authorID);
    Book GetBook(int bookId);
    BookDto GetBookDto(int bookId);
    IEnumerable<Book> GetBooks();//With no options
    IEnumerable<Book> GetBooks(AdvancedBookFilteringOptions searchParameters);
    Task<IEnumerable<Book>> GetBooks(BookFilteringOptions filterOptions);
    IEnumerable<string> GetGenres();
}
