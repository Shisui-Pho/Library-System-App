using LibrarySystem.Models;

namespace LibrarySystem.Utils;
public static class CollectionExtensions
{
    public static IEnumerable<T> ApplyBookDetails<T>(this IEnumerable<T> collection, Func<T,Book> function)
        where T : CartItem
    {
        foreach (var item in collection)
        {
            var book = function(item);
            item.BookInCart = book;
            yield return item;
        }
    }
}//class
