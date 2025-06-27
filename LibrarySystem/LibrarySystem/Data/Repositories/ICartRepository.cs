using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;
public interface ICartRepository : IBaseRepository<CartItem>
{
    IEnumerable<CartItem> GetCartOfUser(string userID);
    IEnumerable<CartItem> GetCartOfBooks(int bookId);
}//
