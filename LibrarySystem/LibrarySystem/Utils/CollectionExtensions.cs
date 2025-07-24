using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
namespace LibrarySystem.Utils;
public static class CollectionExtensions
{
    public static IEnumerable<T> ApplyBookDetails<T>(this IEnumerable<T> collection, Func<T, Book> function)
        where T : CartItem
    {
        foreach (var item in collection)
        {
            var book = function(item);
            item.BookInCart = book;
            yield return item;
        }
    }
    public static IEnumerable<T> Apply<T>(this IEnumerable<T> collection, Action<T> action)
    {
        //Apply the values
        foreach (var item in collection)
            action(item);
        return collection;
    }//Apply
    public static IEnumerable<T> Apply<T>(this IEnumerable<T> collection, Func<T, T> replaceAction)
    {
        //Apply the values
        var lst = collection.ToList();
        for(int i = 0; i < lst.Count; i++)
        {
            lst[i] = replaceAction(lst[i]);
        }
        return lst;
    }//Apply
    public static string Join(this IEnumerable<string> list, string separator)
    {
        return string.Join(separator, list);
    }
}//class