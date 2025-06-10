using LibrarySystem.Data.Repositories;

namespace LibrarySystem.Data;

public interface IRepositoryWrapper
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    IMessageReuqestsRepository MessageReuqests { get; }
    IOrderRepository Orders { get; }
    void SaveChanges();
}
