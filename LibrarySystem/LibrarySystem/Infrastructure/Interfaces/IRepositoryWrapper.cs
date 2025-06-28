namespace LibrarySystem.Infrastructure.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    IMessageReuqestsRepository MessageReuqests { get; }
    IOrderRepository Orders { get; }
    ICartRepository Carts { get; }
    void SaveChanges();
}
