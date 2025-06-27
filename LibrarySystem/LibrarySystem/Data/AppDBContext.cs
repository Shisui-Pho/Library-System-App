using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class AppDBContext(DbContextOptions<AppDBContext> options):
    DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<MessageRequest> MessageRequests { get; set; }
    public DbSet<BookOrder> BookOrders { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Create the tables explicity
        modelBuilder.Entity<Book>().ToTable(nameof(Book));
        modelBuilder.Entity<Author>().ToTable(nameof(Author));
        modelBuilder.Entity<MessageRequest>().ToTable(nameof(MessageRequest));
        modelBuilder.Entity<BookOrder>().ToTable(nameof(BookOrder));
        modelBuilder.Entity<CartItem>().ToTable(nameof(CartItem));
        base.OnModelCreating(modelBuilder);
    }//OnModelCreating
}//class