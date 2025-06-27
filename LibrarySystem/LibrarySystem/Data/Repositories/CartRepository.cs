using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories;

public class CartRepository(AppDBContext context) : BaseRepository<CartItem>(context), IBaseRepository<CartItem>, ICartRepository
{
    public IEnumerable<CartItem> GetCartOfUser(string userID)
    {
        return base._dbContext.CartItems.Where(c => c.UserID == userID)
                                        .Include(c => c.BookInCart)
                                        .ThenInclude(c => c.Authors);
    }//GetCartOfBooks

    public IEnumerable<CartItem> GetCartOfBooks(int bookId)
    {
        return base._dbContext.CartItems.Where(c => c.BookID == bookId)
                                .Include(c => c.BookInCart)
                                .ThenInclude(c => c.Authors);
    }//GetCartOfBooks
}//class