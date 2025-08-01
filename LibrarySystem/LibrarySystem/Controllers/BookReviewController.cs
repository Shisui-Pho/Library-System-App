using LibrarySystem.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;
[Authorize(Roles ="Customer")]
public class BookReviewController : Controller
{
    private readonly IBooksService _bookService;

    public BookReviewController(IBooksService bookService)
    {
        this._bookService = bookService;
    }
    [HttpPost]
    public IActionResult SubmitReview(int bookId, int stars, string reviewText)
    {
        var interactions = _bookService.ReviewBook(bookId, stars, reviewText);

        //Return a partial view for the review section
        return PartialView("_BookReviewSection", interactions);
    }//SubmitReview
    [HttpPost]
    public IActionResult InteracWithComment(int commentId, bool isLiked)
    {
        //Here I need to interact with the book
        var(success, likes, dislikes) = _bookService.CommentInteraction(commentId, isLiked);

        return Json(new { success, likes, dislikes });
    }//InteracWithComment
    [HttpPost]
    public IActionResult UpdateReview(int commentId, int rating, string updatedComment)
    {
        //TODO: Implement update comment feature
        throw new NotImplementedException();
    }
}//class
