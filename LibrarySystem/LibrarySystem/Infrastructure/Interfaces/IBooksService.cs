using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBooksService
{
    IEnumerable<Book> GetBooks();//With no options
    IEnumerable<Book> GetBooks(FilteringOptions filterOptions);
    IEnumerable<Book> GetBooks(AdvanceSearchViewModel searchParameters);
    IEnumerable<string> GetGenres();
}
