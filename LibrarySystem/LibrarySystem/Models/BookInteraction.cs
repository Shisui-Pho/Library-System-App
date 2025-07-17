using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibrarySystem.Models;
public class BookInteraction
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Models.Book))]
    public int BookId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; } = 0;

    #nullable enable
    public Review? Review { get; set; }
    #nullable disable


    // Navigation properties
    public Book Book { get; set; }
    [NotMapped]
    public virtual ApplicationUser User { get; set; }
}//class

[Owned]
public class Review
{
    public string ReviewText { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public int NumberOfLikes { get; set; } = 0;
    public int NumberOfDislikes { get; set; } = 0;
}//class