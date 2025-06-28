using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface ICartRepository : IBaseRepository<CartItem>
{
    IEnumerable<CartItem> GetCartOfUser(string userID);
    IEnumerable<CartItem> GetCartOfBooks(int bookId);
}//
