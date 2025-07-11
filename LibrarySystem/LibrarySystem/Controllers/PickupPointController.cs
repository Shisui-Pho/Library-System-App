using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Controllers;
public class PickupPointController : Controller
{
    private readonly IRepositoryWrapper _repo;

    public PickupPointController(IRepositoryWrapper repo)
    {
        this._repo = repo;
    }
    public IActionResult Index()
    {
        var pickupPoints = _repo.PickupPoints.FindAll();
        return View(pickupPoints);
    }//Index
    [HttpGet]
    public JsonResult GetCitiesForProvince(string province)
    {
        //Get distinct cities for the selected province
        var options = new QueryOptions<PickupPoint>()
        {
            Where = p => p.ProvinceCode == province || p.Province == province,
            OrderBy = p => p.City,
            OrderByDirection = "asc"
        };

        var cities = _repo.PickupPoints.GetWithOptions(options)
                                       .Select(x => x.City)
                                       .Distinct();
                                       
        return Json(cities);
    }
    [HttpGet]
    public JsonResult GetPickupPoints(string province, string city)
    {
        var points = _repo.PickupPoints.FindByCondition(p => (p.ProvinceCode == province || p.Province == province) && p.City == city && p.IsActive)
                                       .Select(p => new {id = p.Id, name = p.Name})
                                       .Distinct()
                                       .ToList();
        return Json(points);
    }//GetPickupPoints
    [HttpGet]
    public JsonResult GetPickupPointDetails(int id)
    {
        var point = _repo.PickupPoints.FindByCondition(p => p.Id == id)
            .Select(p => new {
                id = p.Id,
                name = p.Name,
                address = p.Address,
                phone = p.Phone,
                openingTime = p.OpeningTime.ToString(@"hh\:mm"),
                closingTime = p.ClosingTime.ToString(@"hh\:mm")
            })
            .FirstOrDefault();

        return Json(point);
    }//GetPickupPoint details
}//class