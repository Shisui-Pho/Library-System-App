using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;

public interface IAuthorRepository : IBaseRepository<Author>
{
    IEnumerable<Author> GetAllAuthorsWithBooks();
    Author GetAuthorWithBooks(int id);
}
