using LibrarySystem.Models.Base;

namespace LibrarySystem.Models.DTO;
public class BookDto : BaseBook
{
    public int? AverageRating { get; set; }
    public int? TotalRatings { get; set; }
    public int? TotalReviews { get; set; }
    public int? TotalViews { get; set; }
    public int? TotalLikes { get; set; }
}