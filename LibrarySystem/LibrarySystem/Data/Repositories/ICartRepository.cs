using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;
public interface ICartRepository : IBaseRepository<CartItem>
{
    IEnumerable<CartItem> GetCartItemsOfUser(string userID);
    IEnumerable<CartItem> GetCartOfBookss(int bookId);
}//
