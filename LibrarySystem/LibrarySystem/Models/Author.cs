using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;

public class Author
{
    [Required]
    public int Id { get; set; }
    public bool IsCorporateAuthor { get; set; } = false;
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    public string Biography { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfDeath { get; set; }

    public string Nationality { get; set; }

    public string Website { get; set; }

    public string FullName { get; set; }

    // Navigational property
    public List<Book> Books { get; set; }
    //Slug for URL-friendly paths
    public string Slug => FullName.Replace(" ", "-").ToLowerInvariant();
}
