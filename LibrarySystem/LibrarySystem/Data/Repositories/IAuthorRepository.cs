using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;

public interface IAuthorRepository : IBaseRepository<Author>
{
    IEnumerable<Author> GetAllAuthorsWithBooks();
    Author GetAuthorWithBooks(int id);
}
