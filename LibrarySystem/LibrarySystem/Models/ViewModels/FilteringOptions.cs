namespace LibrarySystem.Models.ViewModels;

public class FilteringOptions
{
    public string Top { get; set; }
    public string? Genre { get; set; }
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public string? PriceRange { get; set; }
    public PagingInfomation Paging { get; set; }
}