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

    // Navigational property
    public List<Author> Authors { get; set; }

    // Computed property for URL-friendly slug
    public string Slug => BookTitle.Replace(" ", "-").ToLowerInvariant();
    public string GetCoverPath() => $"/images/covers/{this.Id}.png"; 
}