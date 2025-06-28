namespace LibrarySystem.Models.ViewModels;
public class BooksDisplayViewModel
{
    public IEnumerable<Book> Books { get; set; }
    public PagingInfomation PagingInfomation { get; set; }
    public string GenerateUniqueIdentifier(int bookID, bool isForAddToChart = false)
    {
        return isForAddToChart ? "addbookNumber" + bookID : "updatebookNumber" + bookID;
    }
}