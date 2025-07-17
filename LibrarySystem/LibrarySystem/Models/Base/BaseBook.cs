namespace LibrarySystem.Models.Base;
public class BaseBook
{
    public virtual int Id { get; set; }

    public virtual required string ISBN { get; set; }

    public virtual required string BookTitle { get; set; }

    public virtual string Description { get; set; }

    public virtual required string Publisher { get; set; }
    public virtual DateTime PublicationDate { get; set; }

    public virtual int PageCount { get; set; }

    public virtual string Language { get; set; }

    public virtual string Genre { get; set; }
    public virtual decimal Price { get; set; }
    public virtual DateOnly AddedDate { get; set; }
    // Computed property for URL-friendly slug
    public virtual string Slug => BookTitle.Replace(" ", "-").ToLowerInvariant();
    public virtual string GetCoverPath() => $"/images/covers/{this.Id}.png";
    public virtual string GetAuthorsString(string namesSeperator = " ", string deliminator = ",")
    {
        if (Authors == null || Authors.Count == 0)
        {
            return "Unknown Author";
        }
        var str = string.Join(deliminator, Authors?.Select(a => a.FirstName + namesSeperator + a.LastName));
        return str;
    }//GetAuthorsString

    // Navigational property
    public List<Author> Authors { get; set; }
    public ICollection<BookInteraction> BookInteractions { get; set; }
}
