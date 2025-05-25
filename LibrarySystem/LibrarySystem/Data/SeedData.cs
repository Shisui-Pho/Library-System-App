using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LibrarySystem.Data;

public class SeedData
{
    //Application will have 3 roles
    //- Customer, Staff and Administrator
    private class MockRegisterViewModel : RegisterViewModel
    {
        //Define extra property for a role
        public string UserRole { get; set; }
    }
    private static readonly string[] roles = ["Customer", "Staff", "Administrator"];
    private static readonly List<MockRegisterViewModel> seedUsers =
    [
        new MockRegisterViewModel
        {
            FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Password = "A1ice@2024",
                ConfirmPassword = "A1ice@2024", ContactNumber = "123-456-7890", UserRole = roles[0]
        },
        new MockRegisterViewModel
        {
            FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", Password = "B0bSecure!",
            ConfirmPassword = "B0bSecure!", ContactNumber = "234-567-8901", UserRole = roles[1]
        },
        new MockRegisterViewModel
        {
            FirstName = "Clara", LastName = "Lee", Email = "clara.lee@example.com", Password = "Cl@ra2025",
            ConfirmPassword = "Cl@ra2025", ContactNumber = "345-678-9012", UserRole = roles[2]
        }
    ];

    public static async Task PopulateLibraryUsersDB(IApplicationBuilder app)
    {
        UserManager<ApplicationUser> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
        RoleManager<IdentityRole> roleManager = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();

        //Get the dbcontext
        AppIdentityDBContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetService<AppIdentityDBContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        //Check the roles database if the roles exist
        if (!context.Roles.Any())
        {
            //Create the roles
            foreach(var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!context.Users.Any())
        {
            foreach (var seedUser in seedUsers)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = seedUser.Email,
                    PhoneNumber = seedUser.ContactNumber,
                    UserName = seedUser.Email, //The username is the same as their email
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName
                };
                await userManager.CreateAsync(user, seedUser.Password);
                await userManager.AddToRoleAsync(user, seedUser.UserRole);
            }
        }

        context.SaveChanges();
    }//PopulateLibraryUsersDB
    public static void PopulateLibrarySystemDB(IApplicationBuilder app)
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
            Description = "Victor Frankenstein, a young scientist, becomes obsessed with creating life. He assembles a creature from body parts and brings it to life, only to be horrified by its grotesque appearance. The creature, abandoned and shunned, seeks companionship but is met with fear and violence. It ultimately turns against its creator, leading to a tragic pursuit across the world. The novel explores themes of ambition, responsibility, and the consequences of playing god.",
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
            Description = "Ishmael, a sailor, joins the whaling ship Pequod, captained by the obsessed Ahab. Ahab seeks revenge against Moby Dick, the white whale that maimed him. The journey is fraught with danger, philosophical musings, and encounters with other ships. As the hunt intensifies, Ahab's obsession leads to disaster, culminating in a final confrontation where the ship is destroyed and only Ishmael survives to tell the tale.",
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
            Description = "Elizabeth Bennet navigates societal expectations and family pressures in 19th-century England. She initially dislikes the wealthy and aloof Mr. Darcy but gradually realizes his true character. Meanwhile, her sisters face their own romantic challenges. The novel critiques class distinctions and gender roles while celebrating love and personal growth.",
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
            Description = "Two young lovers from feuding families, Romeo and Juliet, fall in love at first sight. Their secret marriage leads to tragic consequences when misunderstandings and impulsive decisions result in their untimely deaths. The play explores themes of fate, love, and the destructive nature of family conflict.\r\n",
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
            Description = "Alice falls down a rabbit hole into a whimsical world where logic is turned upside down. She encounters peculiar characters like the Cheshire Cat, the Mad Hatter, and the Queen of Hearts. Through bizarre adventures, Alice navigates the strange rules of Wonderland, ultimately waking up to realize it was all a dream.",
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
            Description = "Jay Gatsby, a wealthy and mysterious man, throws lavish parties in hopes of rekindling his romance with Daisy Buchanan. Narrated by Nick Carraway, the novel explores themes of wealth, love, and the American Dream. Gatsby's idealism leads to his downfall, revealing the emptiness of material success.",
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
            Description = "Nora Helmer, a seemingly happy wife, realizes she has been living in a metaphorical dollhouse, controlled by societal expectations and her husband. When her secret financial dealings come to light, she chooses to leave her family to find independence, challenging traditional gender roles.",
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
            Description = "A comedic play about mistaken identities and societal hypocrisy. Jack and Algernon create fictitious identities to escape social obligations, leading to humorous misunderstandings. Wilde satirizes Victorian norms, emphasizing the absurdity of rigid social expectations.",
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
            Description = "This compilation includes all of Shakespeare's plays, sonnets, and poems, showcasing his mastery in various genres—from tragedies like Hamlet and Macbeth to comedies like A Midsummer Night's Dream and Twelfth Night. His works delve into themes of love, power, betrayal, and the complexities of human nature.",
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
            Description = "A complex novel exploring the lives of various characters in a provincial town. Dorothea Brooke seeks intellectual fulfillment, while Dr. Lydgate struggles with ambition and marriage. The novel examines themes of idealism, societal constraints, and personal growth.\r\n",
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
            Description = " The story follows the lives of the four March sisters—Meg, Jo, Beth, and Amy—as they navigate love, ambition, and family struggles during the American Civil War. Jo, the independent and aspiring writer, faces challenges in balancing her dreams with societal expectations.",
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
            Description = "Lucy Honeychurch, a young woman from England, visits Italy and experiences a transformative journey. She struggles between societal expectations and her true desires, ultimately choosing love and personal freedom over convention.",
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
            Description = "During World War II, the OSS distributed this manual to ordinary citizens in Axis-occupied territories, instructing them on subtle acts of sabotage to disrupt enemy operations. The guide encouraged minor, everyday actions—like misplacing tools or giving incorrect directions—to create confusion and hinder the enemy's efficiency.",
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
            Description = "Dorian Gray, a handsome young man, wishes that his portrait would age instead of him, allowing him to live a life of indulgence without consequence. As he descends into a hedonistic lifestyle, his portrait reflects the corruption of his soul, while he remains outwardly youthful, leading to a tragic downfall.",
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
            Description = "Four women from different backgrounds escape their dreary lives in England to spend a transformative month in an Italian villa. As they embrace the beauty of their surroundings, they undergo personal growth, finding joy, friendship, and self-discovery. The novel explores themes of renewal and the healing power of nature.",
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
            Description = "Valancy Stirling, a timid and repressed woman, receives a fatal diagnosis and decides to break free from her controlling family. She embarks on a journey of self-discovery, finding love and independence in the wilderness. The novel is a heartwarming tale of courage, transformation, and the pursuit of happiness.",
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
            Description = "Jonathan Harker visits Count Dracula's castle, only to discover the vampire's sinister plans. Dracula moves to England, spreading terror, while a group led by Van Helsing attempts to stop him. The novel explores themes of fear, sexuality, and the clash between modernity and superstition.",
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
            Description = "Raskolnikov, a poor student, commits murder, believing he is justified in doing so. His psychological torment and interactions with others lead to his eventual confession and redemption. The novel delves into morality, guilt, and the consequences of one's actions.\r\n",
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
            Description = "Dr. Jekyll creates a potion that transforms him into the evil Mr. Hyde. As Hyde's actions grow more violent, Jekyll struggles to control his darker side. The novel examines duality, repression, and the nature of good and evil.",
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
            Description = "A charming depiction of life in a small English town, focusing on the lives of its female residents. The novel explores themes of social change, friendship, and resilience.",
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
            Description = "This epistolary novel follows the journey of Matthew Bramble, a cantankerous but kind-hearted Welsh squire, as he travels through England and Scotland with his family. Through their letters, the characters provide humorous and satirical observations on 18th-century society, medicine, and politics. The novel is known for its witty character interactions and its critique of contemporary British life.",
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
            Description = "This satirical novel follows the exploits of Ferdinand Count Fathom, a cunning and morally ambiguous character who manipulates and deceives those around him. Born into a life of hardship, Fathom uses his charm and intelligence to navigate a world ripe for exploitation. The novel explores themes of vice, virtue, and the nature of morality.\r\n",
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
            Description = "Tom Jones, a foundling raised by the wealthy Squire Allworthy, embarks on a journey of self-discovery after being cast out due to false accusations. His adventures take him across England, where he encounters love, betrayal, and social satire. The novel is celebrated for its intricate plot, vivid characters, and humorous critique of 18th-century society.\r\n",
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
            Description = "Gregor Samsa wakes up to find himself transformed into a giant insect. His family struggles to cope with his condition, leading to his isolation and eventual demise. The novel explores themes of alienation, identity, and societal expectations.",
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
            Description = "A sequel to *The Three Musketeers*, this novel follows the aging musketeers—D'Artagnan, Athos, Porthos, and Aramis—as they reunite amidst political turmoil in France and England. The story is filled with intrigue, adventure, and historical events, including the execution of King Charles I. The musketeers navigate shifting allegiances while facing old enemies and new challenges.",
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
            Description = "Set during the French Revolution, the novel follows Charles Darnay, a nobleman who renounces his aristocratic heritage, and Sydney Carton, a disillusioned lawyer. Their lives intertwine with Lucie Manette, whose father was imprisoned for years. As the revolution intensifies, Carton makes the ultimate sacrifice, taking Darnay’s place at the guillotine. The novel contrasts themes of resurrection, sacrifice, and the brutality of revolution.",
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
            Description = "This picaresque novel follows Roderick Random, a young Scotsman who faces numerous hardships due to his illegitimate birth. His journey takes him across England, France, and the Caribbean, where he encounters deception, adventure, and romance. The novel offers a satirical look at 18th-century society, particularly the British Navy.",
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
            Description = "Wagner’s autobiography details his early years, influences, and struggles as he pursued a career in music. He reflects on his family, education, and the cultural environment that shaped his artistic vision. The book provides insight into his personal and professional development, offering a glimpse into the mind of one of history’s most influential composers.",
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
            Description = "A satirical essay proposing that impoverished Irish families sell their children as food to the wealthy. Swift uses irony to critique British exploitation of Ireland, highlighting the absurdity of economic rationalization at the expense of human dignity. The essay remains a powerful example of political satire.",
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
            Description = "Jane, an orphan, grows into an independent woman and falls in love with Mr. Rochester. Her journey explores themes of love, morality, and self-respect.",
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
            Description = " A philosophical work presenting Nietzsche's ideas on the Übermensch, morality, and the meaning of life. Zarathustra, the protagonist, delivers profound teachings on human existence.",
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
            Description = "Huck Finn, a rebellious boy, escapes his abusive father and sets off on a journey down the Mississippi River with Jim, a runaway slave. Along the way, they encounter conmen, feuding families, and moral dilemmas. Huck struggles with societal expectations and his own conscience, ultimately deciding to help Jim gain freedom. The novel critiques racism, hypocrisy, and the flaws of civilization.",
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