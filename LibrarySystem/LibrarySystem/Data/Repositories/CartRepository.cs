using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories;

public class CartRepository(AppDBContext context) : BaseRepository<CartItem>(context), IBaseRepository<CartItem>, ICartRepository
{
    public IEnumerable<CartItem> GetCartItemsOfUser(string userID)
    {
        return base._dbContext.CartItems.Where(c => c.UserID == userID)
                                        .Include(c => c.BookInCart);
    }//GetCartOfBookss

    public IEnumerable<CartItem> GetCartOfBookss(int bookId)
    {
        return base._dbContext.CartItems.Where(c => c.BookID == bookId)
                                .Include(c => c.BookInCart);
    }//GetCartOfBookss
}//class