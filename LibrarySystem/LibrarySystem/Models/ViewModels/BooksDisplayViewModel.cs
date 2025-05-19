namespace LibrarySystem.Models.ViewModels;
public class BooksDisplayViewModel
{
    public IEnumerable<Book> Books { get; set; }
    public PagingInfomation PagingInfomation { get; set; }
}