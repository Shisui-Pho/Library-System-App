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

        books.Add(new()
        {
            ISBN = "978-0-123456-01-1",
            BookTitle = "Frankenstein; Or, The Modern Prometheus",
            Description = "\"Frankenstein; Or, The Modern Prometheus\" by Mary Wollstonecraft Shelley is a novel written in the early 19th century. The story explores themes of ambition, the quest for knowledge, and the consequences of man's hubris...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 280,
            Language = "en",
            Genre = "Gothic fiction, Horror, Science fiction",
            Authors = new List<Author> { authors[0] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-02-8",
            BookTitle = "Moby Dick; Or, The Whale",
            Description = "\"Moby Dick; Or, The Whale\" by Herman Melville is a novel written in the mid-19th century. The story follows Ishmael, a sailor on a whaling voyage, who seeks adventure...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 635,
            Language = "en",
            Genre = "Adventure, Psychological fiction, Sea stories",
            Authors = new List<Author> { authors[1] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-03-5",
            BookTitle = "Pride and Prejudice",
            Description = "\"Pride and Prejudice\" by Jane Austen is a classic novel written in the early 19th century. The story delves into themes of love, social class, and individual agency...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 432,
            Language = "en",
            Genre = "Romance, Social commentary, Domestic fiction",
            Authors = new List<Author> { authors[2] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-04-2",
            BookTitle = "Romeo and Juliet",
            Description = "\"Romeo and Juliet\" by William Shakespeare is a tragedy likely written during the late 16th century. The play centers on the intense love affair between two young lovers...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 125,
            Language = "en",
            Genre = "Tragedy, Drama, Romance",
            Authors = new List<Author> { authors[3] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-05-9",
            BookTitle = "Alice's Adventures in Wonderland",
            Description = "\"Alice's Adventures in Wonderland\" by Lewis Carroll is a classic children's novel...",
            Publisher = "Browsing: Children & Young Adult Reading",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 201,
            Language = "en",
            Genre = "Children's fiction, Fantasy, Juvenile fiction",
            Authors = new List<Author> { authors[4] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-06-6",
            BookTitle = "The Great Gatsby",
            Description = "\"The Great Gatsby\" by F. Scott Fitzgerald is a novel written in the early 20th century...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 180,
            Language = "en",
            Genre = "Classic, Psychological fiction, American literature",
            Authors = new List<Author> { authors[5] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-07-3",
            BookTitle = "A Doll's House : a play",
            Description = "\"A Doll's House\" by Henrik Ibsen is a three-act play written during the late 19th century...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 96,
            Language = "en",
            Genre = "Drama, Feminist literature, Norwegian literature",
            Authors = new List<Author> { authors[6] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-08-0",
            BookTitle = "The Importance of Being Earnest: A Trivial Comedy for Serious People",
            Description = "\"The Importance of Being Earnest\" by Oscar Wilde is a play written in the late 19th century...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 76,
            Language = "en",
            Genre = "Comedy, Satire, Drama",
            Authors = new List<Author> { authors[7] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-09-7",
            BookTitle = "The Complete Works of William Shakespeare",
            Description = "\"The Complete Works of William Shakespeare\" includes sonnets, comedies, tragedies, and more...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 1250,
            Language = "en",
            Genre = "Drama, Poetry, English literature",
            Authors = new List<Author> { authors[3] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-10-3",
            BookTitle = "Middlemarch",
            Description = "\"Middlemarch\" by George Eliot is a novel written in the mid-19th century...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 890,
            Language = "en",
            Genre = "Literary fiction, Domestic fiction, Victorian novel",
            Authors = new List<Author> { authors[8] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-11-0",
            BookTitle = "Little Women; Or, Meg, Jo, Beth, and Amy",
            Description = "\"Little Women\" by Louisa May Alcott is a classic novel written in the mid-19th century...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 759,
            Language = "en",
            Genre = "Autobiographical fiction, Domestic fiction, Family life, Sisters, Young women",
            Authors = new List<Author> { authors[9] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-12-7",
            BookTitle = "A Room with a View",
            Description = "\"A Room with a View\" by E. M. Forster is a novel exploring social conventions and personal freedom...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 324,
            Language = "en",
            Genre = "British fiction, Humorous stories, Romance, Coming-of-age",
            Authors = new List<Author> { authors[10] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-13-4",
            BookTitle = "Simple Sabotage Field Manual",
            Description = "\"Simple Sabotage Field Manual\" by United States. Office of Strategic Services is a historical guide...",
            Publisher = "Browsing: History - Warfare",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 64,
            Language = "en",
            Genre = "Sabotage, Warfare, Historical documentation",
            Authors = new List<Author> { authors[11] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-14-1",
            BookTitle = "The Picture of Dorian Gray",
            Description = "\"The Picture of Dorian Gray\" by Oscar Wilde is a novel exploring themes of art, beauty, and morality...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 272,
            Language = "en",
            Genre = "Philosophical fiction, Supernatural, Gothic, Psychological fiction",
            Authors = new List<Author> { authors[7] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-15-8",
            BookTitle = "The Enchanted April",
            Description = "\"The Enchanted April\" by Elizabeth Von Arnim is a novel about personal transformation and joy...",
            Publisher = "Bestsellers, American, 1895-1923",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 336,
            Language = "en",
            Genre = "Domestic fiction, Female friendship, Italy, Love stories",
            Authors = new List<Author> { authors[12] }
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-16-5",
            BookTitle = "The Blue Castle: a novel",
            Description = "\"The Blue Castle\" by L. M. Montgomery is a novel centered around Valancy Stirling...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 280,
            Language = "en",
            Genre = "Romance fiction, Self-discovery, Canadian literature",
            Authors = new List<Author> { authors[13] }
        });
        books.Add(new()
        {
            ISBN = "978-0-123456-17-2",
            BookTitle = "Dracula",
            Description = "\"Dracula\" by Bram Stoker is a Gothic horror novel written in the late 19th century...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 418,
            Language = "en",
            Genre = "Gothic fiction, Horror, Vampires, Epistolary fiction",
            Authors = new List<Author> { authors[14] } // Bram Stoker
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-18-9",
            BookTitle = "Crime and Punishment",
            Description = "\"Crime and Punishment\" by Fyodor Dostoyevsky is a novel written in the mid-19th century...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 576,
            Language = "en",
            Genre = "Psychological fiction, Mystery, Russian literature",
            Authors = new List<Author> { authors[15] } // Fyodor Dostoyevsky
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-19-6",
            BookTitle = "The Strange Case of Dr. Jekyll and Mr. Hyde",
            Description = "\"The Strange Case of Dr. Jekyll and Mr. Hyde\" by Robert Louis Stevenson is a novella...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 144,
            Language = "en",
            Genre = "Horror, Psychological fiction, Science fiction",
            Authors = new List<Author> { authors[16] } // Robert Louis Stevenson
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-20-2",
            BookTitle = "Cranford",
            Description = "\"Cranford\" by Elizabeth Cleghorn Gaskell is a novel written in the mid-19th century...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 336,
            Language = "en",
            Genre = "Domestic fiction, Female friendship, Pastoral fiction",
            Authors = new List<Author> { authors[17] } // Elizabeth Cleghorn Gaskell
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-21-9",
            BookTitle = "The Expedition of Humphry Clinker",
            Description = "\"The Expedition of Humphry Clinker\" by Tobias Smollett is a humorous epistolary novel...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 496,
            Language = "en",
            Genre = "Satire, Travel fiction, Epistolary fiction",
            Authors = new List<Author> { authors[18] } // Tobias Smollett
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-22-6",
            BookTitle = "The Adventures of Ferdinand Count Fathom — Complete",
            Description = "\"The Adventures of Ferdinand Count Fathom\" by Tobias Smollett is a satirical novel...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 480,
            Language = "en",
            Genre = "Adventure fiction, Satire, Gothic fiction",
            Authors = new List<Author> { authors[18] } // Tobias Smollett
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-23-3",
            BookTitle = "History of Tom Jones, a Foundling",
            Description = "\"The History of Tom Jones, A Foundling\" by Henry Fielding is a novel written in the early 18th century...",
            Publisher = "Banned Books from Anne Haight's list",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 968,
            Language = "en",
            Genre = "Bildungsroman, Satire, Romantic fiction, Social commentary",
            Authors = new List<Author> { authors[19] } // Henry Fielding
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-24-0",
            BookTitle = "Metamorphosis",
            Description = "\"Metamorphosis\" by Franz Kafka is a novella written during the late 19th century...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 76,
            Language = "en",
            Genre = "Psychological fiction, Metamorphosis",
            Authors = new List<Author> { authors[20] } // Franz Kafka
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-25-7",
            BookTitle = "Twenty Years After",
            Description = "\"Twenty Years After\" by Alexandre Dumas and Auguste Maquet is a historical novel...",
            Publisher = "Adventure",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 720,
            Language = "en",
            Genre = "Historical fiction, Adventure",
            Authors = new List<Author> { authors[21], authors[22] } // Dumas & Maquet
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-26-4",
            BookTitle = "A Tale of Two Cities",
            Description = "\"A Tale of Two Cities\" by Charles Dickens is a historical novel written in the mid-19th century...",
            Publisher = "Browsing: Fiction",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 489,
            Language = "en",
            Genre = "Historical fiction, Revolution, Social injustice",
            Authors = new List<Author> { authors[23] } // Charles Dickens
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-27-1",
            BookTitle = "The Adventures of Roderick Random",
            Description = "\"The Adventures of Roderick Random\" by Tobias Smollett is a novel written in the early 18th century...",
            Publisher = "Best Books Ever Listings",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 512,
            Language = "en",
            Genre = "Picaresque, Sea stories, Satire",
            Authors = new List<Author> { authors[18] } // Tobias Smollett
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-28-8",
            BookTitle = "My Life — Volume 1",
            Description = "\"My Life — Volume 1\" by Richard Wagner is an autobiographical work written in the mid-19th century...",
            Publisher = "Browsing: Biographies",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 432,
            Language = "en",
            Genre = "Autobiography, Biography, Music",
            Authors = new List<Author> { authors[24] } // Richard Wagner
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-29-5",
            BookTitle = "A Modest Proposal",
            Description = "\"A Modest Proposal\" by Jonathan Swift is a satirical essay written in the early 18th century...",
            Publisher = "Browsing: Humour",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 48,
            Language = "en",
            Genre = "Satire, Political humor, Essay",
            Authors = new List<Author> { authors[25] } // Jonathan Swift
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-30-1",
            BookTitle = "Jane Eyre: An Autobiography",
            Description = "\"Jane Eyre: An Autobiography\" by Charlotte Brontë is a novel written in the early 19th century...",
            Publisher = "Browsing: Culture/Civilization/Society",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 532,
            Language = "en",
            Genre = "Bildungsromans, Love stories, Orphans -- Fiction",
            Authors = new List<Author> { authors[26] } // Charlotte Brontë
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-31-8",
            BookTitle = "Thus Spake Zarathustra: A Book for All and None",
            Description = "\"Thus Spake Zarathustra: A Book for All and None\" by Friedrich Wilhelm Nietzsche is a philosophical treatise...",
            Publisher = "Browsing: Philosophy & Ethics",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 320,
            Language = "en",
            Genre = "Philosophy, German, Superman (Philosophical concept)",
            Authors = new List<Author> { authors[27] } // Friedrich Wilhelm Nietzsche
        });

        books.Add(new()
        {
            ISBN = "978-0-123456-32-5",
            BookTitle = "Adventures of Huckleberry Finn",
            Description = "\"Adventures of Huckleberry Finn\" by Mark Twain is a novel likely written in the late 19th century...",
            Publisher = "Banned Books List from the American Library Association",
            PublicationDate = new DateTime(2025, 5, 18),
            PageCount = 366,
            Language = "en",
            Genre = "Adventure stories, Bildungsromans, Race relations -- Fiction",
            Authors = new List<Author> { authors[28] } // Mark Twain
        });
        return books;
    }//Create books
    private static List<Author> CreateAuthors()
    {
        var authors = new List<Author>();
        authors.Add(new()
        {
            FirstName = "Mary Wollstonecraft",
            LastName = "Shelley",
            DateOfBirth = new DateTime(1797, 1, 1),
            DateOfDeath = new DateTime(1851, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "Herman",
            LastName = "Melville",
            DateOfBirth = new DateTime(1819, 1, 1),
            DateOfDeath = new DateTime(1891, 1, 1),
            Nationality = "American"
        });

        authors.Add(new()
        {
            FirstName = "Jane",
            LastName = "Austen",
            DateOfBirth = new DateTime(1775, 1, 1),
            DateOfDeath = new DateTime(1817, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "William",
            LastName = "Shakespeare",
            DateOfBirth = new DateTime(1564, 1, 1),
            DateOfDeath = new DateTime(1616, 1, 1),
            Nationality = "English"
        });

        authors.Add(new()
        {
            FirstName = "Lewis",
            LastName = "Carroll",
            DateOfBirth = new DateTime(1832, 1, 1),
            DateOfDeath = new DateTime(1898, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "F. Scott (Francis Scott)",
            LastName = "Fitzgerald",
            DateOfBirth = new DateTime(1896, 1, 1),
            DateOfDeath = new DateTime(1940, 1, 1),
            Nationality = "American"
        });

        authors.Add(new()
        {
            FirstName = "Henrik",
            LastName = "Ibsen",
            DateOfBirth = new DateTime(1828, 1, 1),
            DateOfDeath = new DateTime(1906, 1, 1),
            Nationality = "Norwegian"
        });

        authors.Add(new()
        {
            FirstName = "Oscar",
            LastName = "Wilde",
            DateOfBirth = new DateTime(1854, 1, 1),
            DateOfDeath = new DateTime(1900, 1, 1),
            Nationality = "Irish"
        });

        authors.Add(new()
        {
            FirstName = "George",
            LastName = "Eliot",
            DateOfBirth = new DateTime(1819, 1, 1),
            DateOfDeath = new DateTime(1880, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "Louisa May",
            LastName = "Alcott",
            DateOfBirth = new DateTime(1832, 1, 1),
            DateOfDeath = new DateTime(1888, 1, 1),
            Nationality = "American"
        });

        authors.Add(new()
        {
            FirstName = "E. M. (Edward Morgan)",
            LastName = "Forster",
            DateOfBirth = new DateTime(1879, 1, 1),
            DateOfDeath = new DateTime(1970, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "",
            LastName = "United States. Office of Strategic Services",
            IsCorporateAuthor = true
        });

        authors.Add(new()
        {
            FirstName = "Elizabeth",
            LastName = "Von Arnim",
            DateOfBirth = new DateTime(1866, 1, 1),
            DateOfDeath = new DateTime(1941, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "L. M. (Lucy Maud)",
            LastName = "Montgomery",
            DateOfBirth = new DateTime(1874, 1, 1),
            DateOfDeath = new DateTime(1942, 1, 1),
            Nationality = "Canadian"
        });

        authors.Add(new()
        {
            FirstName = "Bram",
            LastName = "Stoker",
            DateOfBirth = new DateTime(1847, 1, 1),
            DateOfDeath = new DateTime(1912, 1, 1),
            Nationality = "Irish"
        });

        authors.Add(new()
        {
            FirstName = "Fyodor",
            LastName = "Dostoyevsky",
            DateOfBirth = new DateTime(1821, 1, 1),
            DateOfDeath = new DateTime(1881, 1, 1),
            Nationality = "Russian"
        });

        authors.Add(new()
        {
            FirstName = "Robert Louis",
            LastName = "Stevenson",
            DateOfBirth = new DateTime(1850, 1, 1),
            DateOfDeath = new DateTime(1894, 1, 1),
            Nationality = "Scottish"
        });

        authors.Add(new()
        {
            FirstName = "Elizabeth Cleghorn",
            LastName = "Gaskell",
            DateOfBirth = new DateTime(1810, 1, 1),
            DateOfDeath = new DateTime(1865, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "T. (Tobias)",
            LastName = "Smollett",
            DateOfBirth = new DateTime(1721, 1, 1),
            DateOfDeath = new DateTime(1771, 1, 1),
            Nationality = "Scottish"
        });

        authors.Add(new()
        {
            FirstName = "Henry",
            LastName = "Fielding",
            DateOfBirth = new DateTime(1707, 1, 1),
            DateOfDeath = new DateTime(1754, 1, 1),
            Nationality = "English"
        });

        authors.Add(new()
        {
            FirstName = "Franz",
            LastName = "Kafka",
            DateOfBirth = new DateTime(1883, 1, 1),
            DateOfDeath = new DateTime(1924, 1, 1),
            Nationality = "Austro-Hungarian"
        });

        authors.Add(new()
        {
            FirstName = "Alexandre",
            LastName = "Dumas",
            DateOfBirth = new DateTime(1802, 1, 1),
            DateOfDeath = new DateTime(1870, 1, 1),
            Nationality = "French"
        });

        authors.Add(new()
        {
            FirstName = "Auguste",
            LastName = "Maquet",
            DateOfBirth = new DateTime(1813, 1, 1),
            DateOfDeath = new DateTime(1888, 1, 1),
            Nationality = "French"
        });

        authors.Add(new()
        {
            FirstName = "Charles",
            LastName = "Dickens",
            DateOfBirth = new DateTime(1812, 1, 1),
            DateOfDeath = new DateTime(1870, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "Richard",
            LastName = "Wagner",
            DateOfBirth = new DateTime(1813, 1, 1),
            DateOfDeath = new DateTime(1883, 1, 1),
            Nationality = "German"
        });

        authors.Add(new()
        {
            FirstName = "Jonathan",
            LastName = "Swift",
            DateOfBirth = new DateTime(1667, 1, 1),
            DateOfDeath = new DateTime(1745, 1, 1),
            Nationality = "Irish"
        });

        authors.Add(new()
        {
            FirstName = "Charlotte",
            LastName = "Brontë",
            DateOfBirth = new DateTime(1816, 1, 1),
            DateOfDeath = new DateTime(1855, 1, 1),
            Nationality = "British"
        });

        authors.Add(new()
        {
            FirstName = "Friedrich Wilhelm",
            LastName = "Nietzsche",
            DateOfBirth = new DateTime(1844, 1, 1),
            DateOfDeath = new DateTime(1900, 1, 1),
            Nationality = "German"
        });

        authors.Add(new()
        {
            FirstName = "Mark",
            LastName = "Twain",
            DateOfBirth = new DateTime(1835, 1, 1),
            DateOfDeath = new DateTime(1910, 1, 1),
            Nationality = "American"
        });

        return authors;
    }//Create authors
}//class