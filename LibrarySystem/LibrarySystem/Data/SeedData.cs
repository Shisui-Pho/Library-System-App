using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;

namespace LibrarySystem.Data;

public class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        AppDBContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<AppDBContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        var authors = CreateAuthors();


        if (!context.Authors.Any())
        {
            //Add authors
            context.Authors.AddRange(authors);
        };
        if (!context.Authors.Any())
        {
            //Add books
            var books = CreateBooks(authors);
            context.Books.AddRange(books);

        }
        context.SaveChanges();
    }//EnsurePopulant
    private static List<Book> CreateBooks(List<Author> authors)
    {
        var books = new List<Book>();

        var book1 = new Book
        {
            ISBN = "9780261103573",
            BookTitle = "The Fellowship of the Ring",
            Description = "First volume of The Lord of the Rings.",
            Publisher = "George Allen & Unwin",
            PublicationDate = new DateTime(1954, 7, 29),
            PageCount = 423,
            Language = "en",
            Genre = "Fantasy",
            Authors = [ authors[0] ]
        };
        authors[0].Books.Add(book1);
        books.Add(book1);

        var book2 = new Book
        {
            ISBN = "9780553294385",
            BookTitle = "Foundation",
            Description = "Classic science fiction novel by Isaac Asimov.",
            Publisher = "Gnome Press",
            PublicationDate = new DateTime(1951, 6, 1),
            PageCount = 255,
            Language = "en",
            Genre = "Science Fiction",
            Authors = [ authors[1]]
        };
        authors[1].Books.Add(book2);
        books.Add(book2);

        var book3 = new Book
        {
            ISBN = "9780451524935",
            BookTitle = "1984",
            Description = "Dystopian novel about surveillance and totalitarianism.",
            Publisher = "Secker & Warburg",
            PublicationDate = new DateTime(1949, 6, 8),
            PageCount = 328,
            Language = "en",
            Genre = "Dystopian",
            Authors = [ authors[2] ]
        };
        authors[2].Books.Add(book3);
        books.Add(book3);

        var book4 = new Book
        {
            ISBN = "9780160915720",
            BookTitle = "NASA Systems Engineering Handbook",
            Description = "Official NASA guide for systems engineering.",
            Publisher = "NASA",
            PublicationDate = new DateTime(2016, 1, 1),
            PageCount = 298,
            Language = "en",
            Genre = "Technical",
            Authors = [ authors[3] ]
        };
        authors[3].Books.Add(book4);
        books.Add(book4);

        var book5 = new Book
        {
            ISBN = "9780141439471",
            BookTitle = "Frankenstein",
            Description = "Gothic novel about a scientist who creates a monster.",
            Publisher = "Lackington, Hughes, Harding, Mavor & Jones",
            PublicationDate = new DateTime(1818, 1, 1),
            PageCount = 280,
            Language = "en",
            Genre = "Gothic Fiction",
            Authors = [ authors[4] ]
        };
        authors[4].Books.Add(book5);
        books.Add(book5);
        return books;
    }//Create books
    private static List<Author> CreateAuthors()
    {
        var authors = new List<Author>
        {
            new() {
                FirstName = "J.R.R.",
                LastName = "Tolkien",
                IsCorporateAuthor = false,
                Biography = "English writer, best known for The Lord of the Rings.",
                DateOfBirth = new DateTime(1892, 1, 3),
                DateOfDeath = new DateTime(1973, 9, 2),
                Nationality = "British",
                Website = "https://www.tolkien.co.uk",
                Books = []
            },
            new() {
                FirstName = "Isaac",
                LastName = "Asimov",
                IsCorporateAuthor = false,
                Biography = "American author and professor of biochemistry, known for works of science fiction.",
                DateOfBirth = new DateTime(1920, 1, 2),
                DateOfDeath = new DateTime(1992, 4, 6),
                Nationality = "American",
                Website = "https://asimovonline.com",
                Books = []
            },
            new() {
                FirstName = "George",
                LastName = "Orwell",
                IsCorporateAuthor = false,
                Biography = "British novelist and journalist, author of 1984 and Animal Farm.",
                DateOfBirth = new DateTime(1903, 6, 25),
                DateOfDeath = new DateTime(1950, 1, 21),
                Nationality = "British",
                Website = "",
                Books = []
            },
            new() {
                FirstName = "NASA",
                LastName = "Publications",
                IsCorporateAuthor = true,
                Biography = "National Aeronautics and Space Administration publication division.",
                DateOfBirth = new DateTime(1958, 10, 1),
                DateOfDeath = DateTime.MinValue,
                Nationality = "American",
                Website = "https://www.nasa.gov",
                Books = []
            },
            new() {
                FirstName = "Mary",
                LastName = "Shelley",
                IsCorporateAuthor = false,
                Biography = "English novelist, author of Frankenstein.",
                DateOfBirth = new DateTime(1797, 8, 30),
                DateOfDeath = new DateTime(1851, 2, 1),
                Nationality = "British",
                Website = "",
                Books = []
            }
        };
        return authors;
    }//Create authors
}//class