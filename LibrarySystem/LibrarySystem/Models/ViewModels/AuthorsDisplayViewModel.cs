namespace LibrarySystem.Models.ViewModels;
public class AuthorsDisplayViewModel
{
    public IEnumerable<Author> Authors { get; set; }
    public PagingInfomation PagingInformation { get; set; }
    public int CountAuthorBooks(int id)
    {
        return Authors?.FirstOrDefault(a => a.Id == id)?.Books?.Count ?? 0;
    }//CountAuthorBooks
}//class
