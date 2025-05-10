using LibrarySystem.Models;

namespace LibrarySystem.Data;

public interface IAuthorRepository : IBaseRepository<Author>
{
    IEnumerable<Author> GetAllAuthorsWithBooks();
    Author GetAuthorWithBooks(int id);
}
