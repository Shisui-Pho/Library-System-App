using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Enums;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.DTO;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace LibrarySystem.Infrastructure.Services;
public class BooksService : IBooksService
{
    private readonly IRepositoryWrapper _repo;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    public BooksService(IRepositoryWrapper repository, IUserService userService, UserManager<ApplicationUser> userManager)
    {
        this._repo = repository;
        this._userService = userService;
        this._userManager = userManager;
    }
    public IEnumerable<Book> GetBooks()
    {
        return _repo.Books.GetAllBooksWithDetails();
    }//GetBooks

    public async Task<IEnumerable<Book>> GetBooks(BookFilteringOptions filterOptions)
    {
        PagingInfomation paging = new()
        {
            CurrentPageNumber = filterOptions.Page,
            NumberOfItemsPerPage = filterOptions.PageSize 
        };
        filterOptions.Paging = paging;
        var books = await _repo.Books.GetBooksByFilter(filterOptions);
        return books;
    }//GetBooks
    public IEnumerable<Book> GetBooks(AdvancedBookFilteringOptions searchParameters)
    {
        throw new NotImplementedException("Advance search is not implemented yet.");
    }//GetBooks
    public IEnumerable<string> GetGenres()
    {
        AdvancedQueryOptions<Book, IEnumerable<string>> options = new()
        {
            Select = p => p.Genres.Select(g => g.Name),
            SelectDistinct = true
        };

        //Select the genres
        var genres = _repo.Books.GetWithOptions(options);
        var lstGenre = ToSingleGenreList(genres);
        
        return lstGenre;
    }
    private IEnumerable<string> ToSingleGenreList(IEnumerable<IEnumerable<string>> genres)
    {
        HashSet<string> result = [];
        foreach (var lsGenreSet in genres)
        {
            foreach(var genre in lsGenreSet)
            {
                result.Add(genre);
            }
        }
        return result;
    }//ToSingleGenreList
    public Book GetBook(int bookId)
    {
        return _repo.Books.GetBookWithDetails(bookId);
    }
    public BookDto GetBookDto(int bookId)
    {
        var book = _repo.Books.GetBookDto(bookId);
        if(book != null)
        {
            book.BookInteractions = book.BookInteractions;
        }
        return book;
    }//GetBookDto
    public (Author author, IEnumerable<Book> books) GetAuthorWithBooks(int authorID)
    {
        var author = _repo.Authors.GetById(authorID);
        if (author == null)
            return (author, []);

        var options = new QueryOptions<Book>()
        {
            Where = b => b.Authors.Any(a => a.Id == authorID),
            OrderBy = b => b.BookTitle,
            OrderByDirection = "asc"
        };

        var authorBooks = _repo.Books.GetWithOptions(options);
        return (author, authorBooks);
    }//GetAuthorWithBooks
    public IEnumerable<Author> GetAuthors(AuthorFilteringOptions filteringOptions)
    {
        var paging = new PagingInfomation()
        {
            CurrentPageNumber = filteringOptions.Page,
            NumberOfItemsPerPage = filteringOptions.PageSize
        };

        var options = new QueryOptions<Author>
        {
            OrderBy = p => EF.Property<Author>(p, filteringOptions.PropertyName),
            OrderByDirection = filteringOptions.SortDirection,
            PagingInfomation = paging,
            Where = a => string.IsNullOrEmpty(filteringOptions.SearchTerm) || 
                         a.FullName.ToLower().Contains(filteringOptions.SearchTerm.ToLower())
        };

        var authors = _repo.Authors.GetWithOptions(options);

        filteringOptions.Paging = options.PagingInfomation;
        return authors;
    }//GetAuthors

    public IEnumerable<BookInteraction> ReviewBook(ClaimsPrincipal claims, int bookId, int rating, string reviewText)
    {
        //TODO : Make this method asynchronous 
        var user = _userService.GetCurrentLoggedInUserAsync(claims).Result;
        if (user == null)
            return [];

        var review = new BookInteraction()
        {
            AddedToWishlist = false,
            BookId = bookId,
            Rating = rating,
            Review = new Review()
            {
                LastUpdated = DateTime.Now,
                NumberOfDislikes = 0,
                NumberOfLikes = 0,
                ReviewText = reviewText
            },
            UserId = user.Id,
            FullUsername = user.FirstName + " " + user.LastName,
            Viewed = true
        };

        //Find the book
        var book = _repo.Books.GetBookWithDetails(bookId);
        if(book == null)
            return [];

        //Add the interactions
        book.BookInteractions.Add(review);
        try
        {
            _repo.Books.Update(book);
            _repo.SaveChanges();

            // TODO : Make method syncronous 
            return book.BookInteractions;
        }
        catch(DbUpdateException ex) 
        {
            //This will be logged somewhere else later on
            _ = ex;
            return [];
        }
    }//ReviewBook

    public (bool success, int likes, int dislikes) CommentInteraction(ClaimsPrincipal user, int commentId, bool isLiked)
    {
        //First get the user id
        var userId = _userService.GetUserId(user);
        
        //Assume the user was found
        var reviewInteraction = _repo.ReviewInteractions
                                     .FindByCondition(i => i.ReviewId == commentId && i.UserId == userId)
                                     .FirstOrDefault();

        //I need to make sure that the review also exists
        var review = _repo.BookInteractions.GetById(commentId);

        if(review == null || review.Review == null)
        {
            //Review was not found
            return (false, 0, 0);
        }

        if(reviewInteraction == null)
        {
            //I need to create a new one
            reviewInteraction = new ReviewInteraction()
            {
                ReviewId = commentId,
                InteractionType = isLiked ? ReviewInteractionType.Like : ReviewInteractionType.Dislike,
                InterectedReview = review,
                UserId = userId
            };

            //Add the interaction to the interaction table
            _repo.ReviewInteractions.Create(reviewInteraction);

            //Increament the number of likes or dislikes
            review.Review.NumberOfLikes = isLiked ? review.Review.NumberOfLikes++ : review.Review.NumberOfDislikes;
            review.Review.NumberOfDislikes = !isLiked ? review.Review.NumberOfDislikes++: review.Review.NumberOfLikes++;
        }
        else
        {
            //Update the interaction
            var statusBefore = reviewInteraction.InteractionType;
            reviewInteraction.InteractionType = isLiked ? ReviewInteractionType.Like : ReviewInteractionType.Dislike;
            _repo.ReviewInteractions.Update(reviewInteraction);

            //need to adjust the likes accordingly
            if(statusBefore  == ReviewInteractionType.Dislike && isLiked)
            {
                review.Review.NumberOfLikes++;
                review.Review.NumberOfDislikes--;
            }
            if(statusBefore == ReviewInteractionType.Like && !isLiked)
            {
                review.Review.NumberOfLikes--;
                review.Review.NumberOfDislikes++;
            }

            //Otherwise we do nothing
        }
        //Update the review state
        _repo.BookInteractions.Update(review);
        //Save the changes
        try
        {
            _repo.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            _ = ex;
            //TODO: Log error somewhere
        }
        return (true, review.Review.NumberOfLikes, review.Review.NumberOfDislikes);
    }
    //public IEnumerable<>
}//class
