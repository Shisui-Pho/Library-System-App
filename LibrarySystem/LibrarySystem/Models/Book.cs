using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

public class Book
{
    [Required]
    public int Id { get; set; }

    [Required]
    public required string ISBN { get; set; }

    [Required]
    public required string BookTitle { get; set; }

    public string Description { get; set; }

    [Required]
    public required string Publisher { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; } // When it was published

    public int PageCount { get; set; } // Number of pages

    public string Language { get; set; } // e.g., "en", "fr"

    public string Genre { get; set; } // e.g., "Fantasy", "Science Fiction"
    [Precision(18,2)]
    public decimal Price { get; set; }
    public DateOnly AddedDate { get; set; }

    // Navigational property
    public List<Author> Authors { get; set; }

    // Computed property for URL-friendly slug
    public string Slug => BookTitle.Replace(" ", "-").ToLowerInvariant();
    public string GetCoverPath() => $"/images/covers/{this.Id}.png"; 
    public string GetAuthorsString(string namesSeperator = " ", string deliminator = ",")
    {
        var str = string.Join(deliminator, Authors?.Select(a => a.FirstName + namesSeperator + a.LastName));
        return str;
    }//GetAuthorsString
}