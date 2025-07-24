using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LibrarySystem.Models.ViewModels;

public class BookFilteringOptions
{
    public string Top { get; set; }
    public string? Genre { get; set; }
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public string? PriceRange { get; set; }
    public PagingInfomation Paging { get; set; }
    public int PageSize { get; set; } = 16;
}//BookFilteringOptions
public class AuthorFilteringOptions
{
    public int PageSize { get; set; }
    public int Page { get; set; }
    public string SearchTerm { get; set; }
    public string PropertyName { get; set; }
    public PagingInfomation Paging { get; set; }
    public string SortDirection { get; set; }
}//AuthorFilteringOptions
public class AdvancedBookFilteringOptions
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string ISBN { get; set; }
    public DateTime? PublicationDateFrom { get; set; }
    public int RatingFrom { get; set; } = 0; //default rating
    public string Keywords { get; set; }
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public string Language { get; set; }
} //class