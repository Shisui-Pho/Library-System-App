using LibrarySystem.Data;
using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Enums;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.DTO;
using LibrarySystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Services;
public class DashboardService : IDashboardService
{
    private readonly IUserService _userService;
    private readonly IRepositoryWrapper _repo;
    private readonly AppIdentityDBContext _identity;

    public DashboardService(IRepositoryWrapper repo, IUserService userService, AppIdentityDBContext identity)
    {
        this._userService = userService;
        this._repo  = repo;
        this._identity = identity;
    }
    #nullable enable
    public async Task<AdminStaffDashboardViewModel?> GetDashboardDetailsModel()
    {
        if (!_userService.IsLoggedIn())
            return null;

        var model = new AdminStaffDashboardViewModel();

        //Build model
        await SetBookStats(model);
        await SetOrderStats(model);
        await SetUserStats(model);
        SetSystemAlerts(model);
        SetRevenueStats(model);


        //Set the current user
        var currentUser = await _userService.GetCurrentLoggedInUserAsync();
        if (currentUser != null)
        {
            model.Name = currentUser.FirstName + " " + currentUser.LastName;
        }

        return model;
    }//GetDashboardDetailsModel
    private async Task SetBookStats(AdminStaffDashboardViewModel model)
    {
        int totalBooks = _repo.Books.Count();

        var dtSevenDays = DateTime.Now.AddDays(-7);
        var dtoSevenDays = new DateOnly(dtSevenDays.Year, dtSevenDays.Month, dtSevenDays.Day);
        int newBooksThisWeek = _repo.Books.FindByCondition(book => book.AddedDate >= dtoSevenDays).Count();

        var topGenres = await _repo.Books.GetTopNGenres(5);

        model.TotalBooks = totalBooks;
        model.NewBooksThisWeek = newBooksThisWeek;
        model.TopGenres = topGenres;
    }
    private async Task SetOrderStats(AdminStaffDashboardViewModel model)
    {
        //Orders that will need special attention will be orders with either have a priority or
        // - orders that are pending for more than 3 days
        int numberOfOrdersThatNeedAttention = _repo.Orders.Count(order => order.Status == BookOrderStatus.Pending && order.CreatedAt < DateTime.Now.AddDays(-3));
        int numberOfCancelledOrders = _repo.Orders.Count(order => order.Status == BookOrderStatus.Cancelled);
        int numberOfShippedOrders = _repo.Orders.Count(order => order.Status == BookOrderStatus.Shipped);
        int numberOfOrdersBeingProcessed = _repo.Orders.Count(order => order.Status == BookOrderStatus.Processing);
        
        var queryOptions = new QueryOptions<Order>
        {
            OrderBy = order => order.CreatedAt,
            Where = o => o.Status == BookOrderStatus.Pending,
            OrderByDirection = "dsc", 
            PagingInfomation = new() { CurrentPageNumber = 1, NumberOfItemsPerPage = 5 }
        };
        var recentOrders = _repo.Orders.GetWithOptions(queryOptions);
        //This will get all the number of pending orders
        int numberOfPendingOrders = queryOptions.PagingInfomation.TotalNumberOfItems;

        model.NumberOfPendingOrders = numberOfPendingOrders;
        model.NumberOfOrdersThatNeedAttention = numberOfOrdersThatNeedAttention;
        model.NumberOfCancelledOrders = numberOfCancelledOrders;
        model.NumberOfShippedOrders = numberOfShippedOrders;
        model.NumberOfOrdersBeingProcessed = numberOfOrdersBeingProcessed;

        await SetUsersInOrders(recentOrders);

        model.RecentOrders = recentOrders;
    }
    private async Task SetUserStats(AdminStaffDashboardViewModel model)
    {
        //This will get the number of active users
        var active = await _identity.loggedInUsers.Select(x => x.UserId)
                                        .Distinct().ToListAsync();

        var numberOfActiveUsers = active.Count;

        var numberOfNewUsers = await _identity.Users
                                        .Where(user => user.RegisteredDate >= DateTime.Now.AddDays(-30))
                                        .CountAsync();

        //var user = _userService.GetCurrentLoggedInUserAsync()

        model.NumberOfActiveUsers = numberOfActiveUsers;
        model.NumberOfNewUsers = numberOfNewUsers;
    }
    private void SetRevenueStats(AdminStaffDashboardViewModel model)
    {
        //This will get the total revenue for the last 30 days

        var dtThirtyDays = DateTime.Now.AddDays(-30);
        var revenueAmount = _repo.Orders.FindByCondition(order => order.CreatedAt >= dtThirtyDays && order.Status == BookOrderStatus.Shipped)
                                        .Sum(order => order.TotalPrice);

        model.TotalRevenue = revenueAmount;
        //This will get the percentage growth for the last 30 days
        var percentageGrowth = _repo.Orders.FindByCondition(order => order.CreatedAt >= dtThirtyDays && order.Status == BookOrderStatus.Shipped)
                                        .Select(order => order.TotalPrice)
                                        .DefaultIfEmpty(0)
                                        .Average();
        model.PercentageGrowth = (double)percentageGrowth;
    }
    private static void SetSystemAlerts(AdminStaffDashboardViewModel model)
    {
        //Create some test data for system alerts
        var systemAlerts = new List<SystemAlert>
        {
            new() {
                Id = 1,
                Title = "System Maintenance",
                Description = "The system will undergo maintenance on Saturday at 2 AM.",
                CreatedAt = DateTime.Now,
                SeverityLevel = SystemAlertSeverityLevel.Severe
            },
            new ()
            {
                Id = 2,
                Title = "New Feature Release",
                Description = "A new feature has been released that allows users to track their reading progress.",
                CreatedAt = DateTime.Now.AddDays(-1),
                SeverityLevel = SystemAlertSeverityLevel.None
            },
            new ()
            {
                Id = 3,
                Title = "Security Update",
                Description = "A security update is available. Please update your application to the latest version.",
                CreatedAt = DateTime.Now.AddDays(-2),
                SeverityLevel = SystemAlertSeverityLevel.Mild
            }
        };
        model.SystemAlerts = systemAlerts;
    }//SetSystemAlerts
    private async Task SetUsersInOrders(IEnumerable<Order> orders)
    {
        foreach(var order in orders)
        {
            var user = await _identity.Users.Where(u => u.Id == order.UserID).FirstOrDefaultAsync();

            if(user != null)
            {
                order.User = user;
            }
            else
            {
                //If the user is not found, set it to null
                order.User = null;
            }
        }//end foreach
    }//SetUsersInOrders
}//class