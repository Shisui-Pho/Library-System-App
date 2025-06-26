using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure;
public interface ICartService
{
    CartViewModel GetCart(HttpContext context);
    (bool success, int total) AddToCart(HttpContext context, CartItemViewModel item);
    (bool success, int total) RemoveFromCart(HttpContext context, int bookID);
}
