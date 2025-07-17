namespace LibrarySystem.Models.ViewModels;

public class AdvanceSearchViewModel
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