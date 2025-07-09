namespace LibrarySystem.Models.ViewModels;
public class BookAuthorViewModel(List<(int bookId, int quantity)> items)
{
    public IEnumerable<Book> Books { get; set; }
    public Author Author { get; set; }
    public int NumberOfBooksByAuthor => Books?.Count() ?? 0;
    public CartItemStatus CartItemStatus { get; } = new CartItemStatus(items);
}
