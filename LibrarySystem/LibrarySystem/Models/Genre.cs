namespace LibrarySystem.Models;
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // Navigation property for the many-to-many relationship with Book
    public ICollection<Book> Books { get; set; } 
}
