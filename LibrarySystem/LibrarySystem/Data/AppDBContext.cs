using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class AppDBContext(DbContextOptions<AppDBContext> options):
    DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Create the tables explicity
        modelBuilder.Entity<Book>().ToTable(nameof(Book));
        modelBuilder.Entity<Author>().ToTable(nameof(Author));
        base.OnModelCreating(modelBuilder);
    }//OnModelCreating
}//class