using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface ICartService
{
    int CountCartItems (HttpContext context);
    CartViewModel GetCart(HttpContext context);
    (bool success, int total) AddToCart(HttpContext context, CartItemViewModel item);
    (bool success, int total) RemoveFromCart(HttpContext context, int bookID);
    (bool success, int total) UpdateQuantity(HttpContext context,int bookId, int quantity);
}
