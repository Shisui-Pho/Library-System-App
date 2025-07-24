using LibrarySystem.Infrastructure.Enums;
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
    public bool Viewed { get; set; } = false;
    public bool AddedToWishlist { get; set; } = false;
    [Required]
    //This will be the name displayed for their comments/reviews
    public string FullUsername { get; set; }
    #nullable enable
    public Review? Review { get; set; }
    #nullable disable


    // Navigation properties
    public Book Book { get; set; }
}//class

[Owned]
public class Review
{
    public string ReviewText { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public int NumberOfLikes { get; set; } = 0;
    public int NumberOfDislikes { get; set; } = 0;

}//class

//This class is necessary to prevent spamming for a review
public class ReviewInteraction
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Models.BookInteraction))]
    public int ReviewId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public ReviewInteractionType InteractionType { get; set; }


    // Navigation property for the book review
    //-Ofcourse here we are not actually referring to the Review itself, but the interaction
    
    public BookInteraction InterectedReview { get; set; }
}