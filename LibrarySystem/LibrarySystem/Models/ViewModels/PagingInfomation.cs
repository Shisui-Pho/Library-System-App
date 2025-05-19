namespace LibrarySystem.Models.ViewModels;
public class PagingInfomation
{
    public int CurrentPageNumber { get; set; }
    public int TotalNumberOfItems { get; set; } = 1;
    public int NumberOfItemsPerPage { get; set; } = 1;
    public int TotalNumberOfPages
        => (int)Math.Ceiling((double)TotalNumberOfItems / NumberOfItemsPerPage);
}