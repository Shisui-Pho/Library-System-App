using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

public class HomeController : Controller
{
    private readonly IRepositoryWrapper _repository;
    public HomeController(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Contact()
    {
        //Pass in a new message request
        return View(new MessageRequest());
    }
    [HttpPost]
    public IActionResult Contact(MessageRequest messageRequest)
    {
        //Check the model state
        if(!ModelState.IsValid)
        {
            return View(messageRequest);
        }

        //-Create a new message request
        _repository.MessageReuqests.Create(messageRequest);
        _repository.SaveChanges();

        //-Pass in a temporary messsage to alert the user that the message has bee sent
        TempData["Message"] = "Your message was sent successfully. A stuff member will reach out to you shortly through the provided details.";
        //Return the same view
        return RedirectToAction("Contact");
    }
    public IActionResult PageNotFound(string message)
    {
        return View(message);
    }//PageNotFound
}