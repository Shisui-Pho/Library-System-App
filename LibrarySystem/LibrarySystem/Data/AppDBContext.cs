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
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Create the tables explicity to avoid pluralization
        modelBuilder.Entity<Book>().ToTable(nameof(Book));
        modelBuilder.Entity<Author>().ToTable(nameof(Author));
        modelBuilder.Entity<MessageRequest>().ToTable(nameof(MessageRequest));
        modelBuilder.Entity<BookOrder>().ToTable(nameof(BookOrder));
        modelBuilder.Entity<CartItem>().ToTable(nameof(CartItem));
        modelBuilder.Entity<PickupPoint>().ToTable(nameof(PickupPoint));
        modelBuilder.Entity<PaymentMethod>().ToTable(nameof(PaymentMethod));
        modelBuilder.Entity<DeliveryAddress>().ToTable(nameof(DeliveryAddress));
        base.OnModelCreating(modelBuilder);
    }//OnModelCreating
}//class